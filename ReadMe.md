# HConfigs

A flexible, Reflection-based configuration system

HConfigs is built around the Reflection system, and uses attributes to turn a plain class into a set of options that can be automatically and easily saved, loaded, and read. Config classes are saved to the disk as human-readable text files, and by default support string, int, long, float, double, and bool parameters. HConfigs also supports custom parsers for any type you might want.

## Using HConfigs
### Creating a Config
To define a config, simply mark a class with the Config property. Mark the properties of that class you want to use as configuration options with the Config.Option property, or set UseAllProperties in the Config property to automatically include all properties in the class. 
The Config, Config.Option, and Config.Comment attributes all have properties that can be used to organize and format the config file that they generate.

### Saving and Loading
The static methods Config.Save() and Config.Load() are sufficient for most uses to save and load config files. If you want to handle the file IO in a different way, Config.Serialize() and Config.Parse() generate and consume the string arrays that would normally make up the config files.

### Custom Parsers
You can extend the range of available option types by writing custom parsers, which inherit from the ParserBase type. To use a custom parser, register it with the Config.RegisterParser() static method.

### Custom Writers
You can override the formatting of config files (except for how options and their values are written) by writing a custom Writer that implements IConfigWriter. To use a custom writer, register it with the Config.RegisterWriter() static method.
*Note: Your writer (as well as comments and other text) must not use the "<tag>[Option Name]: Option Value" pattern used to detect options in the file.*
