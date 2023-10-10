using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckEmptyLinesInProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the path to the .NET project.");
                return;
            }

            string projectPath = args[0];

            if (!Directory.Exists(projectPath))
            {
                Console.WriteLine("Provided path does not exist.");
                return;
            }

            string[] csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);

            var regex = new Regex(@"(\r?\n\s*){3,}", RegexOptions.Compiled);

            foreach (var file in csFiles)
            {
                var content = await File.ReadAllTextAsync(file);
                if (regex.IsMatch(content))
                {
                    Console.WriteLine($"File {file} has more than two consecutive empty lines. Cleaning up...");

                    // Replacing more than two consecutive empty lines with just two.
                    var cleanedContent = regex.Replace(content, "\n\n");

                    // Write the cleaned content back to the file.
                    await File.WriteAllTextAsync(file, cleanedContent);
                    Console.WriteLine($"Cleaned up {file}.");
                }
            }
        }
    }
}