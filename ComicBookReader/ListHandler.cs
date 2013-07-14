using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NananananaCBR
{
    class ListHandler
    {                
        private List<string> fullNames;        
        private List<string> fileNames;

        public ListHandler(string Directory)
        {
            DirectoryInfo dir;
            dir = new DirectoryInfo(Directory);            
            List<FileInfo> fileInfo = new List<FileInfo>();
            fullNames = new List<string>();
            fileNames = new List<string>();
            foreach (FileInfo f in fileInfo)
            {
                if (Regex.Match
                    (f.FullName, Constants.Strings.RegExPattern, RegexOptions.IgnoreCase).
                    Success == true)
                {
                    fullNames.Add(f.FullName);
                    fileNames.Add(f.Name);
                }
            }
        }

        public Dictionary<string, string[]> getFileNames()
        {
            Dictionary<string, string[]> namesDictionary = new Dictionary<string, string[]>();
            namesDictionary.Add("full", fullNames.ToArray());
            namesDictionary.Add("file", fileNames.ToArray());
            //string str = namesDictionary["full"][0];
            return namesDictionary;
        }

        public int getCount()
        {
            return fullNames.Count();
        }
    }
}
