using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArguments arguments = CommandLineArguments.Instance;
            if (!CommandLine.Parser.Default.ParseArguments(args, arguments))
            {
                arguments.InputFilePath = "movementList.csv";
                arguments.OutputFilePath = "alfa-statement.csv";
                arguments.InputSeparator = arguments.OutputSeparator = ";";
            }

            List<AlfaItem> results = new List<AlfaItem>();

            bool isFirstLine = true;

            string inputPath = Path.IsPathRooted(arguments.InputFilePath) ? arguments.InputFilePath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arguments.InputFilePath);
            foreach (string line in File.ReadLines(inputPath, Encoding.Default))
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                results.Add(new AlfaItem(line));
            }

            string outputPath = Path.IsPathRooted(arguments.OutputFilePath) ? arguments.OutputFilePath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arguments.OutputFilePath);
            using (FileStream stream = File.Create(outputPath))
            {
                OutputGenerator generator = new OutputGenerator(results);
                generator.Write(stream);
            }
        }
    }
}
