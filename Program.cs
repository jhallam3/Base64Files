using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Base64Files
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var path = args[0];
                //var path = "/Users/jonhallam/Downloads";
            
                //var path = "/Users/jonhallam/Downloads/0000-004   34.xml";
            
            
            
                var isDirectory = IsDirectory(path);
                var files = Array.Empty<string>();
                string json;
                if (isDirectory) 
                {
                    files = Directory.GetFiles(path);
                    List<Base64File> lb64F = new List<Base64File>();
                    
                    foreach (var file in files)
                    {
                        var content = GetFileAndContents(file);
                        if (content != null)
                        {
                            lb64F.Add(content);
                        }
                    }
                    json = JsonConvert.SerializeObject(lb64F, Formatting.Indented);
                }
                else
                {
                    files = new string[] {path};
                    var singleFile = GetFileAndContents(files.ElementAt((0)));
                    json = JsonConvert.SerializeObject(singleFile, Formatting.Indented);
                    
                }
               
                Console.WriteLine(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("either a full path or a full file location needs to be added to the end of the commandline tool");
                throw;
            }
        }

        private static Base64File GetFileAndContents(string file)
        {
            if (file.Contains(".DS_Store") || file.Contains(".localized")) return null;
            string filename;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                filename = file.Split("\\").Last();
            }
            else
            {
                filename = file.Split("/").Last();
            }

            var returnData = new Base64File
            {
                Contents = Convert.ToBase64String(File.ReadAllBytes(file)),
                FileName = filename
            };

            
            return returnData;

        }
        
        private static bool IsDirectory(string path)
        {
            var fa = File.GetAttributes(path);
            return (fa & FileAttributes.Directory) != 0;
        }
    }
}