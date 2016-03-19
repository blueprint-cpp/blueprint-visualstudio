using System;

namespace Blueprint.VisualStudio
{

    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Blueprint.VisualStudio");

            string solutionFile = @"E:\depot_git\archive\Bloody\Bloody.Engine.sln";

            SolutionConverter converter = new SolutionConverter();
            converter.ConvertSolution(solutionFile);
        }
    }
}
