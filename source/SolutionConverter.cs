using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Blueprint.VisualStudio
{
    class SolutionConverter
    {
        public void ConvertSolution(string solutionFile)
        {
            string solutionBuffer = Microsoft.Build.BuildEngine.SolutionWrapperProject.Generate(solutionFile, null, null);
            byte[] byteBuffer = Encoding.Unicode.GetBytes(solutionBuffer.ToCharArray());

            using (var stream = new MemoryStream(byteBuffer))
            using (var reader = new XmlTextReader(stream))
            {
                var projectCollection = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection;

                projectCollection.SetGlobalProperty("Configuration", "Release");

                ConvertSolution(solutionFile, projectCollection.LoadProject(reader));
            }
        }

        private void ConvertSolution(string solutionFile, Microsoft.Build.Evaluation.Project solution)
        {
            if (solution == null)
            {
                return;
            }

            var projectCollection = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection;

            projectCollection.SetGlobalProperty("SolutionDir", solution.GetPropertyValue("SolutionDir"));
            projectCollection.SetGlobalProperty("SolutionFileName", solution.GetPropertyValue("SolutionFileName"));
            projectCollection.SetGlobalProperty("SolutionName", solution.GetPropertyValue("SolutionName"));
            projectCollection.SetGlobalProperty("SolutionPath", solution.GetPropertyValue("SolutionPath"));

            Console.WriteLine("> solution: " + solutionFile);

            foreach (var item in solution.Items)
            {
                ConvertProject(solutionFile, item);
            }
        }

        private void ConvertProject(string solutionFile, Microsoft.Build.Evaluation.ProjectItem projectItem)
        {
            var projectFile = Path.Combine(Path.GetDirectoryName(solutionFile), projectItem.EvaluatedInclude);
            if (!File.Exists(projectFile))
            {
                return;
            }

            var project = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.LoadProject(projectFile);
            if (project == null)
            {
                return;
            }

            Console.WriteLine(">> project: " + projectFile);

            List<string> files = new List<string>();

            foreach (var item in project.GetItems("ClInclude"))
            {
                files.Add(item.EvaluatedInclude);
            }

            foreach (var item in project.GetItems("ClCompile"))
            {
                files.Add(item.EvaluatedInclude);
            }

            files.Sort();

            foreach (var file in files)
            {
                Console.WriteLine(">>> " + file);
            }
        }
    }
}
