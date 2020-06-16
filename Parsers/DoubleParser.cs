using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class DoubleParser : ParserBase
    {
        public DoubleParser() : base("Double", typeof(double))
        {
        }

        public override object Decode(string data)
        {
            return double.Parse(data);
        }

        public override string Encode(object data)
        {
            return data.ToString();
        }
    }
}
