using System;
using System.Collections.Generic;

namespace Blueprint.VisualStudio
{
    class Project
    {
        public string Name { get; set; }
        public string File { get; set; }

        public List<Configuration> Configurations = new List<Configuration>();
        public List<string> Files = new List<string>();

        public override string ToString()
        {
            return Name;
        }
    }
}
