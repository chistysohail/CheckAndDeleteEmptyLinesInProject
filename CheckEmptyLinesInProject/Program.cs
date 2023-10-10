using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string projectPath = args[0];  // Get the project path from the command-line arguments.

        string[] csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories); // Find all C# files in the project directory and its subdirectories.

        foreach (var file in csFiles)
        {
            string text = await File.ReadAllTextAsync(file);  // Read the file content.

            // Regex to identify more than two consecutive line breaks, preserving the indentation (if any) of the subsequent line.
            var regex = new Regex(@"(?:\r?\n|\r){3,}(?=\s*[\S])");

            // Replace instances of the pattern with a single line break, keeping the indentation of the subsequent line.
            string newText = regex.Replace(text, Environment.NewLine + Environment.NewLine);

            if (!text.Equals(newText))
            {
                await File.WriteAllTextAsync(file, newText);  // Write the modified content back to the file.
                Console.WriteLine($"Modified file: {file}"); // Log the modified file to the console.
            }
        }

        Console.WriteLine("Processing completed.");
    }
}