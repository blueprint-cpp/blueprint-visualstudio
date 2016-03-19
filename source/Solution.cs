using System;
using System.Collections.Generic;

namespace Blueprint.VisualStudio
{
    class Solution
    {
        public string Name { get; set; }
        public string File { get; set; }

        public List<Project> Projects = new List<Project>();

        public override string ToString()
        {
            return Name;
        }
    }
}
