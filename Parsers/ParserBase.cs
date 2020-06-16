using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    public abstract class ParserBase
    {
        /// <summary>
        /// The identifying tag used in config files to specify this type of option
        /// </summary>
        public string Tag { get; }
        /// <summary>
        /// The type that this converter is responsible for
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Encodes an option value to a string to be written to the file. Passing this string to Decode() should return the original value of 'data'
        /// </summary>
        /// <param name="data">The value to encode</param>
        /// <returns>A string representation of 'data'</returns>
        public abstract string Encode(object data);

        /// <summary>
        /// Decodes a string to an opion value of a specific type. Passing this object to Encode() should return the original string 'data'
        /// </summary>
        /// <param name="data">The string to decode</param>
        /// <returns>An object of the type this Parser is responsible for, whose value is represented by 'data'</returns>
        public abstract object Decode(string data);

        /// <summary>
        /// Create a new Parser, with the specified Tag and Type.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        protected ParserBase(string tag, Type type)
        {
            Tag = tag;
            Type = type;
        }
    }
}
