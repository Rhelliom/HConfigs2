using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class LongParser : ParserBase
    {
        public LongParser() : base("Long", typeof(long))
        {
        }

        public override object Decode(string data)
        {
            return long.Parse(data);
        }

        public override string Encode(object data)
        {
            return data.ToString();
        }
    }
}
