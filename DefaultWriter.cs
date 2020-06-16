using System;
using System.Collections.Generic;
using System.Text;

namespace HConfigs
{
    class DefaultWriter : IConfigWriter
    {
        public void Comment(List<string> content, string comment)
        {
            string[] comments = comment.Split('\r', '\n');
            for(int i = 0; i < comments.Length; i++)
            {
                if (!string.IsNullOrEmpty(comments[i]))
                    content.Add($"#{comments[i]}");
            }
        }

        public void Header(List<string> content, string header)
        {
            string[] lines = header.Split('\r', '\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                    content.Add($">---{lines[i]}---<");
            }
        }

        public void Region(List<string> content, string region)
        {
            string[] lines = region.Split('\r', '\n');
            for(int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                    content.Add($"###{lines[i]}###");
            }
        }
    }
}
