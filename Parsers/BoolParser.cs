using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class BoolParser : ParserBase
    {
        public BoolParser() : base("Bool", typeof(bool))
        {
        }

        public override object Decode(string data)
        {
            return bool.Parse(data);
        }

        public override string Encode(object data)
        {
            return data.ToString();
        }
    }
}
