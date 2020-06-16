using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HConfigs
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class Config : Attribute
    {
        public string File { get; }

        public string Header { get; set; }

        public bool UseAllProperties { get; set; }

        public Config(string path)
        {
            File = path;
        }

        //Static Methods + properties
        private static Dictionary<Type, ParserBase> Parsers = new Dictionary<Type, ParserBase> {
            { typeof(int), new IntParser() },
            { typeof(long), new LongParser() },
            { typeof(string), new StringParser() },
            { typeof(double), new DoubleParser() },
            { typeof(float), new FloatParser() },
            { typeof(bool), new BoolParser()}
        };

        /// <summary>
        /// Register a custom option parser. 
        /// </summary>
        /// <param name="parser">Registers the parser, if the associated type does not already have a parser registered.</param>
        /// <param name="replace">If set to true, the parser will be registered in place of an existing parser for its type.</param>
        public static void RegisterParser(ParserBase parser, bool replace = false) {
            if (replace || !Parsers.ContainsKey(parser.Type))
                Parsers[parser.Type] = parser;
        }

        private static IConfigWriter Writer = new DefaultWriter();

        /// <summary>
        /// Register a custom file writer
        /// </summary>
        /// <param name="writer">The custom file writer to user</param>
        public static void RegisterWriter(IConfigWriter writer)
        {
            Writer = writer;
        }

        private const string _allowedCharacters = "[a-zA-Z0-9_ ]+";

        public static Config GetAttribute<T>()
        {
            object[] cfgAttr = typeof(T).GetCustomAttributes(typeof(Config), true);
            if (cfgAttr.Length != 1)
                throw new ConfigException($"Type {typeof(T)} is either not marked as a Config or has multiple Config attributes");

            return (Config)cfgAttr[0];
        }

        public static string[] Serialize<T>(T config)
        {
            Config configAttribute = GetAttribute<T>();

            //Get options
            List<string> regions = new List<string> { "" };
            List<KeyValuePair<PropertyInfo, Option>> options = new List<KeyValuePair<PropertyInfo, Option>>();
            foreach (PropertyInfo member in typeof(T).GetProperties())
            {
                object[] attr = member.GetCustomAttributes(typeof(Option), true);
                Option optionAttribute = attr.Length > 0 ? (Option)attr[0] : null;
                if(optionAttribute != null || configAttribute.UseAllProperties)
                {
                    options.Add(new KeyValuePair<PropertyInfo, Option>(member, (optionAttribute == null ? new Option () : optionAttribute)));
                    if (optionAttribute != null && !regions.Contains(optionAttribute.Region))
                        regions.Add(optionAttribute.Region);

                }
            }
            options.Sort((a, b) =>
            {
                
                if (a.Value.Region.CompareTo(b.Value.Region) != 0)
                    return a.Value.Region.CompareTo(b.Value.Region);
                else
                {
                    string aName = string.IsNullOrEmpty(a.Key.Name) ? a.Key.Name : a.Key.Name;
                    string bName = string.IsNullOrEmpty(b.Key.Name) ? b.Key.Name : b.Key.Name;
                    return aName.CompareTo(bName);
                }
            });

            //Get comments
            List<KeyValuePair<string, string>> comments = new List<KeyValuePair<string, string>>();
            foreach (object attr in typeof(T).GetCustomAttributes(typeof(Comment), true)){
                Comment comment = (Comment)attr;
                comments.Add(new KeyValuePair<string, string>(comment.Region, comment.Text));
                if (!regions.Contains(comment.Region))
                    regions.Contains(comment.Region);
            }
            comments.Sort((a, b) =>
            {
                return a.Key.CompareTo(b.Key);
            });

            regions.Sort();

            //Write content
            List<string> lines = new List<string>();
            if (!string.IsNullOrEmpty(configAttribute.Header))
                Writer.Header(lines, configAttribute.Header);

            foreach(string r in regions)
            {
                if (!string.IsNullOrEmpty(r))
                    Writer.Region(lines, r);

                foreach(var comment in comments.Where(c => c.Key.Equals(r))){
                    Writer.Comment(lines, comment.Value);
                }

                foreach(var option in options.Where(o => o.Value.Region.Equals(r)))
                {
                    if (!string.IsNullOrEmpty(option.Value.Comment))
                        Writer.Comment(lines, option.Value.Comment);

                    Type type = option.Key.PropertyType;
                    string name = string.IsNullOrEmpty(option.Value.Name) ? option.Key.Name : option.Value.Name;
                    if (!Parsers.ContainsKey(type))
                        throw new ConfigException($"No parser registered for type {type}!");
                    ParserBase parser = Parsers[type];
                    if (!Regex.IsMatch(name, $"^{_allowedCharacters}$"))
                        throw new ConfigException($"The specified option name ({name}) is invalid.");
                    if (!Regex.IsMatch(parser.Tag, $"^{_allowedCharacters}$"))
                        throw new ConfigException($"The supplied parser for type {type} has an invalid tag: '{parser.Tag}' contains illegal characters.");
                    string value = parser.Encode(option.Key.GetValue(config));
                    lines.Add($"<{parser.Tag}>[{name}]: {value}");
                }
            }

            return lines.ToArray();

        }

        public static void Save<T>(T config, string path = "")
        {
            System.IO.File.WriteAllLines(path + GetAttribute<T>(), Serialize(config));
        }

        public static T Parse<T>(IEnumerable<string> lines) where T : new()
        {
            T config = (T)Activator.CreateInstance(typeof(T));

            foreach (string line in lines)
            {
                Match match = Regex.Match(line, $"^<{_allowedCharacters}>\\[{_allowedCharacters}\\]:");
                if (match.Success)
                {
                    string optionString = match.Value;
                    string valueString = line.Substring(optionString.Length);

                    string typeTag = Regex.Match(optionString, $"<{_allowedCharacters}>").Value;
                    typeTag = typeTag.Substring(1, typeTag.Length - 2);
                    string optionName = Regex.Match(optionString, $"\\[{_allowedCharacters}\\]").Value;
                    optionName = optionName.Substring(1, optionName.Length - 2);

                    bool successflag = false;
                    foreach(PropertyInfo property in typeof(T).GetProperties())
                    {
                        ParserBase parser = null;
                        foreach(ParserBase p in Parsers.Values)
                        {
                            if (p.Tag.Equals(typeTag))
                            {
                                parser = p;
                                break;
                            }
                        }
                        if (parser == null)
                            throw new ConfigException($"No parser found with tag '{typeTag}'");

                        object[] attr = property.GetCustomAttributes(typeof(Option), true);
                        Option optionAttribute = attr.Length > 0 ? (Option)attr[0] : null;
                        //if ((optionAttribute != null && ((optionAttribute.Name != null && optionName.Equals(optionAttribute.Name)) || )) || (configAttribute.UseAllProperties && property.Name.Equals(optionName)))
                        if(optionAttribute != null)
                        {
                            if (!string.IsNullOrEmpty(optionAttribute.Name))
                            {
                                if (optionAttribute.Name.Equals(optionName))
                                {
                                    property.SetValue(config, parser.Decode(valueString.Trim()));
                                    successflag = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (property.Name.Equals(optionName))
                                {
                                    property.SetValue(config, parser.Decode(valueString.Trim()));
                                    successflag = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (property.Name.Equals(optionName))
                            {
                                property.SetValue(config, parser.Decode(valueString.Trim()));
                                successflag = true;
                                break;
                            }
                        }
                    }
                    if (!successflag)
                        throw new ConfigException($"Failed to map option {optionName} to a property of {typeof(T)}");

                }
            }
            return config;
        }

        public static T Load<T>(string path = "") where T : new()
        {
            Config configAttribute = GetAttribute<T>();
            return Parse<T>(System.IO.File.ReadAllLines(path + configAttribute.File));
        }

        //Sub-attributes
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class Option : Attribute
        {
            public string Name { get; set; } = null;
            public string Comment { get; set; } = null;
            public string Region { get; set; } = "";
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
        public class Comment : Attribute
        {
            public string Text { get; }
            public string Region { get; set; } = "";

            public Comment(string comment)
            {
                Text = comment;
            }
        }
    }
}
