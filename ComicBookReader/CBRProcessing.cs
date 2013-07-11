using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ComicBookReader.Nomenclatures;

namespace ComicBookReader
{
    class CBRProcessing
    {
        private int page;
        private SevenZipExtractor sze;        
        private Collection<string> mutableArchiveNames;
        private MemoryStream ms;
        private BitmapImage bitmap;

        ~CBRProcessing()
        {
            ms.Dispose();
            sze.Dispose();
        }
        public CBRProcessing(string fileDirectory)
        {
            sze = new SevenZipExtractor(fileDirectory);
            page = 0;            
            mutableArchiveNames = new Collection<string>();
            ms = new MemoryStream();
            ReadOnlyCollection<string> archiveNames = sze.ArchiveFileNames;
            foreach (string s in archiveNames)
            {
                if (Regex.Match
                    (s, Constants.Strings.RegExPattern, RegexOptions.IgnoreCase).
                    Success == true)
                {
                    mutableArchiveNames.Add(s);
                }
            }
            archiveNames = null;            
            sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
            ms.Seek(0, SeekOrigin.Begin);
            SetImage();
        }

        private BitmapImage SetImage()
        {
            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            return bitmap;
        }

        public BitmapImage GetImage()
        {
            if (bitmap != null)
                return bitmap;
            else return null;
        }

        public BitmapImage GetNext()
        {
            if (page < mutableArchiveNames.Count - 1)
                page++;
            ms.Dispose();
            ms = new MemoryStream();            
            sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
            ms.Seek(0, SeekOrigin.Begin);                        
            return SetImage();
        }

        public BitmapImage GetPrevious()
        {
            if (page > 0)
                page--;
            ms.Dispose();
            ms = new MemoryStream();
            sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
            ms.Seek(0, SeekOrigin.Begin);
            return SetImage();
        }
    }
}
