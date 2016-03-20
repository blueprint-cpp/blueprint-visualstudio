using System;
using System.Collections.Generic;
using System.IO;

namespace Blueprint.VisualStudio
{
    class Project
    {
        public string Name { get; set; }
        public string File { get; set; }

        public List<Configuration> Configurations = new List<Configuration>();
        public List<string> Files = new List<string>();

        public void MakeRelative(string basePath)
        {
            var projectPath = Path.GetDirectoryName(File);

            Configurations.ForEach(c => c.MakeRelative(basePath));
            Files = Files.ConvertAll(f => Utility.MakePathRelative(basePath, f));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
