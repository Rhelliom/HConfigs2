using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    public interface IConfigWriter
    {
        public void Header(List<string> content, string header);

        public void Comment(List<string> content, string comment);

        public void Region(List<string> content, string region);
    }
}
