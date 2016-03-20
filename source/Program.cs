using System;
using System.IO;

namespace Blueprint.VisualStudio
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Blueprint.VisualStudio");

            if (args.Length > 0)
            {
                string solutionFile = args[0];

                Console.WriteLine("Filename: {0}", solutionFile);

                SolutionImporter importer = new SolutionImporter();
                Solution solution = importer.ImportSolution(solutionFile);

                //solution.MakeRelative(Directory.GetCurrentDirectory());

                JsonExporter.ExportSolution(solution);
            }
            else
            {
                Console.WriteLine("Error: missing solution file argument");
            }
        }
    }
}
