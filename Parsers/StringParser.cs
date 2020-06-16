using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class StringParser : ParserBase
    {
        public StringParser() : base("String", typeof(string))
        {
        }

        public override object Decode(string data)
        {
            return data;
        }

        public override string Encode(object data)
        {
            return (string)data;
        }
    }
}
