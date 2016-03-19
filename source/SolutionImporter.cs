using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Blueprint.VisualStudio
{
    class SolutionImporter
    {
        public Solution ImportSolution(string solutionFile)
        {
            string solutionBuffer = Microsoft.Build.BuildEngine.SolutionWrapperProject.Generate(solutionFile, null, null);
            byte[] byteBuffer = Encoding.Unicode.GetBytes(solutionBuffer.ToCharArray());

            using (var stream = new MemoryStream(byteBuffer))
            using (var reader = new XmlTextReader(stream))
            {
                var projectCollection = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection;

                projectCollection.SetGlobalProperty("Configuration", "Release");

                return ImportSolution(solutionFile, projectCollection.LoadProject(reader));
            }
        }

        private Solution ImportSolution(string solutionFile, Microsoft.Build.Evaluation.Project msbuildSolution)
        {
            if (msbuildSolution  == null)
            {
                return null;
            }

            var projectCollection = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection;

            string toolsVersion = projectCollection.Toolsets.FirstOrDefault().ToolsVersion;

            projectCollection.SetGlobalProperty("SolutionDir", msbuildSolution.GetPropertyValue("SolutionDir"));
            projectCollection.SetGlobalProperty("SolutionFileName", msbuildSolution.GetPropertyValue("SolutionFileName"));
            projectCollection.SetGlobalProperty("SolutionName", msbuildSolution.GetPropertyValue("SolutionName"));
            projectCollection.SetGlobalProperty("SolutionPath", msbuildSolution.GetPropertyValue("SolutionPath"));

            Solution solution = new Solution()
            {
                Name = Path.GetFileNameWithoutExtension(solutionFile),
                File = solutionFile
            };

            Console.WriteLine("> solution: " + solutionFile);

            foreach (var item in msbuildSolution.Items)
            {
                ImportProject(solutionFile, toolsVersion, solution, item);
            }

            return solution;
        }

        private void ImportProject(string solutionFile, string toolsVersion, Solution solution, Microsoft.Build.Evaluation.ProjectItem projectItem)
        {
            var projectFile = Path.Combine(Path.GetDirectoryName(solutionFile), projectItem.EvaluatedInclude);
            if (!File.Exists(projectFile))
            {
                return;
            }

            var msbuildProject = Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.LoadProject(projectFile, toolsVersion);
            if (msbuildProject == null)
            {
                return;
            }

            Project project = new Project()
            {
                Name = Path.GetFileNameWithoutExtension(projectFile),
                File = projectFile
            };

            //project.Configurations.Add(new Configuration() { Name = "Debug" });
            //project.Configurations.Add(new Configuration() { Name = "Release" });

            solution.Projects.Add(project);

            Console.WriteLine(">> project: " + projectFile);

            List<string> files = new List<string>();

            foreach (var item in msbuildProject.GetItems("ClInclude"))
            {
                files.Add(NormalizePath(item.EvaluatedInclude));
            }

            foreach (var item in msbuildProject.GetItems("ClCompile"))
            {
                files.Add(NormalizePath(item.EvaluatedInclude));
            }

            files.Sort();
            project.Files.AddRange(files);

            foreach (var file in files)
            {
                Console.WriteLine(">>> " + file);
            }
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }
    }
}
