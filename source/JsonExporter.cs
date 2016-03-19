using System;
using System.IO;

using Newtonsoft.Json.Linq;

namespace Blueprint.VisualStudio
{
    class JsonExporter
    {
        public static void ExportSolution(Solution solution)
        {
            if (solution == null)
            {
                return;
            }

            JObject jsonSolution = new JObject();
            JArray jsonProjects = new JArray();

            foreach (var project in solution.Projects)
            {
                ExportProject(project);

                jsonProjects.Add(project.Name + ".prj.json");
            }

            jsonSolution.Add(new JProperty("solution", solution.Name));
            jsonSolution.Add(new JProperty("solutionfile", solution.File.Replace('\\', '/')));
            jsonSolution.Add(new JProperty("projects", jsonProjects));

            File.WriteAllText(solution.Name + ".wks.json", jsonSolution.ToString());
        }

        public static void ExportProject(Project project)
        {
            if (project == null)
            {
                return;
            }

            JObject jsonProject = new JObject();
            JArray jsonConfigs = new JArray();

            foreach (var config in project.Configurations)
            {
                JObject jsonConfig = new JObject();

                jsonConfig.Add(new JProperty("name", config.Name));
                jsonConfig.Add(new JProperty("defines", config.Defines));
                jsonConfig.Add(new JProperty("includedirs", config.Includes));
                jsonConfig.Add(new JProperty("pchsource", config.PrecompiledHeader));

                jsonConfigs.Add(jsonConfig);
            }

            jsonProject.Add(new JProperty("project", project.Name));
            jsonProject.Add(new JProperty("projectfile", project.File.Replace('\\', '/')));
            jsonProject.Add(new JProperty("configs", jsonConfigs));
            jsonProject.Add(new JProperty("files", project.Files));

            File.WriteAllText(project.Name + ".prj.json", jsonProject.ToString());
        }
    }
}
