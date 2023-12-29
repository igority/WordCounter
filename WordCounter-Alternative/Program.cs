using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordCounter_Alternative
{
    public class Program
    {
        private static ConcurrentDictionary<string, int> _wordCountResult;

        public static async Task Main(string[] args)
        {
            string workingDirectory = (args.Length > 0) ? args[0] : ReadDirectoryFromInput();
            await ProcessFilesAndOutput(workingDirectory);
        }

        private static string ReadDirectoryFromInput()
        {
            Console.WriteLine("Enter directory path (leave empty for default):");
            var directoryInput = Console.ReadLine();
            return !string.IsNullOrEmpty(directoryInput?.Trim())
                ? directoryInput
                : @$"{Environment.CurrentDirectory}..\..\..\..\..\TestingFolders\test1";
        }

        private static async Task ProcessFilesAndOutput(string workingDirectory)
        {
            try
            {
                await ProcessFiles(workingDirectory);
                if (!_wordCountResult.IsEmpty)
                {
                    WriteOutput();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static async Task ProcessFiles(string workingDirectory)
        {
            _wordCountResult = new ConcurrentDictionary<string, int>();
            string[] filePaths = Directory.GetFiles(workingDirectory, "*.txt");
            if (filePaths.Length == 0)
            {
                Console.WriteLine("No text files found. Exiting application");
                return;
            }
            Console.WriteLine($"Processing {filePaths.Length} file(s)...");

            var lineTasks = new ConcurrentQueue<Task>();

            var fileTasks = filePaths.Select(filePath =>
            {
                return Task.Run(async () =>
                {
                    var lineTasksPerFile = await LineTasks(filePath);
                    foreach (var lineTask in lineTasksPerFile)
                    {
                        lineTasks.Append(lineTask);
                    }
                });

            });

            await Task.WhenAll(fileTasks);
            await Task.WhenAll(lineTasks);

            Console.WriteLine($"Completed.\n");
        }

        private static async Task<IEnumerable<Task>> LineTasks(string filePath)
        {
            string[] lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);

            var tasks = lines.Select(line =>
            {
                return Task.Run(() =>
                {
                    string[] words = Regex.Split(line.ToLower(), @"\W+")
                        .Where(word => !string.IsNullOrWhiteSpace(word))
                        .ToArray();

                    foreach (var word in words)
                    {
                        _wordCountResult.AddOrUpdate(word, 1, (_, count) => count + 1);
                    }
                });
            });
            return tasks;
        }

        private static void WriteOutput()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Number of unique words: {_wordCountResult.Count}");
            Console.WriteLine($"Total number of words: {_wordCountResult.Select(x => x.Value).Sum()}");
            Console.WriteLine($"Aggregated words:");
            foreach (var entry in _wordCountResult.OrderByDescending(res => res.Value))
            {
                Console.WriteLine($"{entry.Value}: {entry.Key}");
            }
            Console.ResetColor();
        }
    }
}
