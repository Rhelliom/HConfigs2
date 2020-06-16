using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    public abstract class ParserBase
    {
        public string Tag { get; }
        public Type Type { get; }

        public abstract string Encode(object data);

        public abstract object Decode(string data);

        protected ParserBase(string flag, Type type)
        {
            Tag = flag;
            Type = type;
        }
    }
}
