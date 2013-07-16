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
        private Dictionary<string, string[]> namesDictionary = new Dictionary<string, string[]>();

        public ListHandler(string Directory)
        {
            DirectoryInfo dir;
            dir = new DirectoryInfo(Directory);
            List<FileInfo> fileInfo = dir.GetFiles().ToList();
            List<string> fullNames = new List<string>();
            List<string> fileNames = new List<string>();
            namesDictionary = new Dictionary<string, string[]>();
            foreach (FileInfo f in fileInfo)
            {
                if (Regex.Match
                    (f.Name, Constants.Strings.RegExPatternForArchives, RegexOptions.IgnoreCase).
                    Success == true)
                {
                    fullNames.Add(f.FullName);
                    fileNames.Add(f.Name);
                }
            }
            namesDictionary.Add(Constants.Strings.FullNameKey, fullNames.ToArray());
            namesDictionary.Add(Constants.Strings.FileNameKey, fileNames.ToArray());
        }        

        public int getCount()
        {
            return namesDictionary[Constants.Strings.FileNameKey].Count(); ;
        }

        public string getFullName(int index)
        {
            try
            {
                return namesDictionary[Constants.Strings.FullNameKey][index];
            }
            catch (IndexOutOfRangeException) {
                return string.Empty;
            }
        }

        public string getFileName(int index)
        {
            try
            {
                return namesDictionary[Constants.Strings.FileNameKey][index];
            }
            catch (IndexOutOfRangeException)
            {
                return string.Empty;
            }
        }
    }
}
