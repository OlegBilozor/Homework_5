using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Homework_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write starter directory name:");
            string directory = Console.ReadLine(); //initial directory
            Console.WriteLine("Write pattern:"); 
            string pattern = Console.ReadLine(); //file filter pattern
            Console.WriteLine("Write minimum date(dd,mm,yyyy):");
            DateTime minDate; //file filter minimum date
            try
            {
                minDate = DateTime.Parse(Console.ReadLine());

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid date format!");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Write maximum date:");
            DateTime maxDate; //file filter maximum date
            try
            {
                maxDate = DateTime.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid date format!");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Choose file to write results:");
            string fileName = Console.ReadLine(); //file name, where results of search will be stored, file will be in project directory
            FileHelper.FindFiles(directory, pattern, minDate, maxDate, fileName); //call the method, which looks for files, writes them in file
            if (FileHelper.searchResult.Count == 0) //if no files were found
            {
                Console.WriteLine("There are no such files!");
            }
            else //if at least one file was found
            {
                Console.WriteLine("Results:");
                FileHelper.searchResult.ForEach(s=>Console.WriteLine(s));
            }

            Console.ReadLine();
        }
        public static class FileHelper //static class for searching operation
        {
            public static List<string> searchResult = new List<string>(); //list to contain results of search to output in console in future
            public static void FindFiles(string path, string pattern, DateTime min, DateTime max, string fileToWrite)
            {

                if (!Directory.Exists(path)) //if directory doesn't exist
                {
                    Console.WriteLine("There is no such directory!");
                    return;
                }
                DirectoryInfo directory = new DirectoryInfo(path); //DirectoryInfo to work with our directory
                using (FileStream fs = new FileStream(fileToWrite, FileMode.Append, FileAccess.Write)) //creating FileStream to write results
                {
                    using (StreamWriter sw = new StreamWriter(fs)) //creating StreamWriter for our FileStream
                    {
                        FileInfo[] files = directory.GetFiles(pattern); //getting all files in the directory we work with that suit our pattern
                        for (int i = 0; i < files.Length; i++)
                        {
                            if ((files[i].LastWriteTime >= min && files[i].LastWriteTime <= max)) //if file LastWriteTime also suits we...  
                            {
                                string file = $"Path: {files[i].DirectoryName}, Name: {files[i].Name}, Last change time: {files[i].LastWriteTime}";
                                sw.WriteLine(file); //write file data to result file...
                                searchResult.Add(file); //and to list of results
                            }
                        }
                    }
                }
                DirectoryInfo[] subDirectories = directory.GetDirectories(); //getting all sub-directories in the directory we work with
                foreach (var directoryInfo in subDirectories)
                {
                    FindFiles(directoryInfo.FullName, pattern, min, max, fileToWrite); //call the method for each of sub-directories
                }
            }
        }
        
    }
}
