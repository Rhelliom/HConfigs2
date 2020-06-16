using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    public interface IConfigWriter
    {
        /// <summary>
        /// Should add formatted header text to the content collection
        /// </summary>
        /// <param name="content">The collection of lines that will be written to the file</param>
        /// <param name="header">The raw header text, as specified in the Config attribute</param>
        public void Header(List<string> content, string header);

        /// <summary>
        /// Should add formatted comment text to the content collection
        /// </summary>
        /// <param name="content">The collection of lines that will be written to the file</param>
        /// <param name="comment">The raw comment text, as specified in attributes</param>
        public void Comment(List<string> content, string comment);

        /// <summary>
        /// Should add a region header to the content collection
        /// </summary>
        /// <param name="content">The collection of lines that will be written to the file</param>
        /// <param name="region">The region name</param>
        public void Region(List<string> content, string region);
    }
}
