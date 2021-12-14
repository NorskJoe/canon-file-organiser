using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace canon_file_organiser
{
    public class FileMover
    {
        private readonly string _rootDirectory;
        private readonly string _targetDirectory;

        public FileMover(string rootDirectory, string targetDirectory)
        {
            _rootDirectory = rootDirectory;
            _targetDirectory = targetDirectory;
        }

        public void Process()
        {
            ProcessDirectories(_rootDirectory);
        }

        private void ProcessDirectories(string currentDirectory)
        {
            Directory.GetFiles(currentDirectory)
                .Where(x => !File.GetAttributes(x).HasFlag(FileAttributes.Hidden))
                .Where(x => !x.EndsWith(".CR2"))
                .GroupBy(x => GetDateTaken(x)?.ToString("yyyy-MM"))
                .ToList()
                .ForEach(grouping =>
                {
                    var newFolder = $"{_targetDirectory}/{grouping.Key}/";
                    Directory.CreateDirectory(newFolder);
                    Directory.CreateDirectory($"{newFolder}/RAW");
                    foreach (var file in grouping)
                    {
                        var fileName = file[file.LastIndexOf("\\")..];
                        File.Move($"{currentDirectory}/{fileName}", $"{newFolder}/{fileName}");
                        
                        var rawFileName = fileName.Replace(".JPG", ".CR2");
                        File.Move($"{currentDirectory}/{rawFileName}", $"{newFolder}/RAW/{rawFileName}");
                    }
                });

            var subDirectories = Directory.GetDirectories(currentDirectory).ToList();
            subDirectories.ForEach(ProcessDirectories);
        }

        private static DateTime? GetDateTaken(string path)
        {
            var regex = new Regex(":");
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var myImage = Image.FromStream(fs, false, false);
            var propItem = myImage.GetPropertyItem(36867);
            if (propItem?.Value == null)
            {
                return null;
            }
            
            var dateTaken = regex.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
            return DateTime.Parse(dateTaken);

        }
    }
}