using System.Text;
using System.Text.RegularExpressions;

namespace WordCounter_SingleThread
{
    public class Program
    {
        private const string DEFAULT_DIRECTORY_RELATIVE_PATH = @"\..\..\..\..\TestingFolders\test1";

        private static Dictionary<string, int> _wordCountResult = new();

        public static async Task Main(string[] args)
        {
            string workingDirectory = args.Length > 0 ? args[0] : ReadDirectoryFromInput();
            await ProcessFilesAndOutput(workingDirectory);
        }

        private static string ReadDirectoryFromInput()
        {
            Console.WriteLine("Enter directory path (leave empty for default):");
            string? directoryInput = Console.ReadLine();
            return !string.IsNullOrEmpty(directoryInput?.Trim())
                ? directoryInput
                : $"{Environment.CurrentDirectory}{DEFAULT_DIRECTORY_RELATIVE_PATH}";
        }

        private static async Task ProcessFilesAndOutput(string workingDirectory)
        {
            try
            {
                await ProcessFiles(workingDirectory);
                if (_wordCountResult.Any())
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
            _wordCountResult = new();
            string[] filePaths = Directory.GetFiles(workingDirectory, "*.txt");
            if (filePaths.Length == 0)
            {
                Console.WriteLine("No text files found. Exiting application");
                return;
            }
            Console.WriteLine($"Processing {filePaths.Length} file(s)...");
            foreach (string filePath in filePaths)
            {
                await ProcessFile(filePath);
            }
            Console.WriteLine($"Completed.\n");
        }

        private static async Task ProcessFile(string filePath)
        {
            string[] lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                string[] words = Regex.Split(line.ToLower(), @"\W+")
                        .Where(word => !string.IsNullOrWhiteSpace(word))
                        .ToArray();
                foreach (var word in words)
                {
                    _wordCountResult[word] = _wordCountResult.ContainsKey(word) ? _wordCountResult[word] + 1 : 1;
                }
            }
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