using System;
using System.Collections.Generic;

namespace Blueprint.VisualStudio
{
    class Solution
    {
        public string Name { get; set; }
        public string File { get; set; }

        public List<Project> Projects = new List<Project>();

        public void MakeRelative(string basePath)
        {
            Projects.ForEach(p => p.MakeRelative(basePath));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
