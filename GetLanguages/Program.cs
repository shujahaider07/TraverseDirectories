using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GetLanguages
{
    public class Program
    { 
        public static void TraverseAndCopyFiles(string sourceDir, string destinationDir)
        {
            Queue<DirectoryInfo> directoryQueue = new Queue<DirectoryInfo>();
            HashSet<string> processedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            directoryQueue.Enqueue(new DirectoryInfo(sourceDir));

            // Ensure the destination directory exists
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            while (directoryQueue.Count > 0)
            {
                DirectoryInfo currentDir = directoryQueue.Dequeue();

                if (processedDirectories.Contains(currentDir.FullName))
                {
                    Console.WriteLine($"Directory already processed: {currentDir.FullName}");
                    continue; // Skip already processed directories
                }

                processedDirectories.Add(currentDir.FullName);
                Console.WriteLine($"Processing directory: {currentDir.FullName}");

                try
                {
                    // Calculate the relative path for the destination directory
                    string relativePath = currentDir.FullName.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar);
                    string destDir = Path.Combine(destinationDir, relativePath);

                    // Ensure the destination subdirectory exists
                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir);
                    }

                    // Copy all files in the current directory to the destination directory
                    FileInfo[] files = currentDir.GetFiles();
                    foreach (var file in files)
                    {
                        string destFile = Path.Combine(destDir, file.Name);
                        file.CopyTo(destFile, true); // 'true' to overwrite existing files
                        Console.WriteLine($"Copied file: {file.FullName} to {destFile}");
                    }

                    // Enqueue all subdirectories in the current directory
                    DirectoryInfo[] subDirs = currentDir.GetDirectories();
                    foreach (var dir in subDirs)
                    {
                        directoryQueue.Enqueue(dir);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing the directory {currentDir.FullName}: {ex.Message}");
                }
            }
        }

        static async Task Main(string[] args)
        {
            string inputDir = @"D:\Parent\";
            string destinationDirectory = @"D:\DestinatedFolde3r";
            TraverseAndCopyFiles(inputDir, destinationDirectory);

            Console.ReadKey();

        }

    }
}

