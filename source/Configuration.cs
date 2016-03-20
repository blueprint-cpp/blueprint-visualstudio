using System;
using System.Collections.Generic;

namespace Blueprint.VisualStudio
{
    class Configuration
    {
        public string Name { get; set; }

        public string PrecompiledHeader { get; set; }

        public List<string> Defines = new List<string>();
        public List<string> Includes = new List<string>();

        public void MakeRelative(string basePath)
        {
            Includes = Includes.ConvertAll(i => Utility.MakePathRelative(basePath, i));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
