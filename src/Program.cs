using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Convertor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter the directory of the project ");
            var projDirectory = Console.ReadLine();

            Console.Write("Enter the capitaized name (E.G) NewWidget ");
            var capitaizedName = Console.ReadLine();

            Console.Write("Enter the uncapitaized name (E.G) newWidget ");
            var uncapitaizedName = Console.ReadLine();

            ReplaceInFileContent(capitaizedName, uncapitaizedName, projDirectory);

            Console.Write("Done");
            Console.ReadKey();
        }
        

        private static void ReplaceInFileContent(string capitaizedName, string uncapitaizedName, string projDirectory)
        {
            Parallel.ForEach(GetFiles(projDirectory), f =>
            {
                Console.WriteLine($"==> Reading {f}");
                var contents = File.ReadAllText(f).Replace("widget", uncapitaizedName).Replace("Widget", capitaizedName);
                var newLocation = f.Replace("widget", uncapitaizedName).Replace("Widget", capitaizedName);
                MakeDirectory(newLocation);
                File.WriteAllText(newLocation, contents);
                Console.WriteLine($"<== Writing {newLocation}");
            });
        }

        private static void MakeDirectory(string newLocation)
        {
            var newDirectory = Path.GetDirectoryName(newLocation);
            if (Directory.Exists(newDirectory) == false)
            {
                lock (typeof(Program))
                {
                    if (Directory.Exists(newDirectory) == false)
                    {
                        Directory.CreateDirectory(newDirectory);
                    }
                }
            }
        }

        private static IEnumerable<string> GetFiles(string projDirectory)
        {
            var files = Directory.GetFiles(projDirectory, "*.*", SearchOption.AllDirectories)
                .OrderBy(x => x.Length);
            return files;
        }
    }
}
