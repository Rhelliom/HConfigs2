using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class IntParser : ParserBase
    {
        public IntParser() : base("Int", typeof(int))
        {
        }

        public override object Decode(string data)
        {
            return int.Parse(data);
        }

        public override string Encode(object data)
        {
            return data.ToString();
        }
    }
}
