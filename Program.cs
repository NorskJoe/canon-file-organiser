using System;
using System.IO;
using System.Linq;

namespace canon_file_organiser
{
    class Program
    {
        private static string _sourceDirectory;
        private static string _targetDirectory;

        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Enter source directory");
                _sourceDirectory = Console.ReadLine();

                Console.WriteLine("Enter target directory");
                _targetDirectory = Console.ReadLine();
            }
            else if (args.Length == 1)
            {
                _sourceDirectory = args[0];
                _targetDirectory = args[0][..(args[0].LastIndexOfAny(new char[] {'\\', '/'})+1)];
            }
            else if (args.Length == 2)
            {
                _sourceDirectory = args[0];
                _targetDirectory = args[1];
            }
            else
            {
                Console.WriteLine("Usage: run -- \"<sourceDirectory>\" \"<targetDirectory>\"");
                return;
            }

            if (!Directory.Exists(_sourceDirectory))
            {
                Console.WriteLine("Source directory does not exist");
                Console.WriteLine($"Source: {_sourceDirectory}");
                return;
            }
            if (!Directory.Exists(_targetDirectory))
            {
                if (_targetDirectory != null) Directory.CreateDirectory(_targetDirectory);
            }

            Console.WriteLine($"Source: {_sourceDirectory}");
            Console.WriteLine($"Target: {_targetDirectory}");

            var folderCreator = new FileMover(_sourceDirectory, _targetDirectory);
            Console.WriteLine("Generating Directory Structure");
            folderCreator.Process();
            Console.WriteLine("Process Complete");
        }
    }
}
