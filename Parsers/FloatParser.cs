using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class FloatParser : ParserBase
    {
        public FloatParser() : base("Float", typeof(float))
        {
        }

        public override object Decode(string data)
        {
            return float.Parse(data);
        }

        public override string Encode(object data)
        {
            return data.ToString();
        }
    }
}
