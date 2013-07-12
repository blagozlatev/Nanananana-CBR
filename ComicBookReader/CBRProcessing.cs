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
    /// <summary>
    /// 
    /// </summary>
    class CBRProcessing:IDisposable
    {
        private int page;
        private SevenZipExtractor sze;        
        private Collection<string> mutableArchiveNames;
        private MemoryStream ms;
        private BitmapImage bitmap;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileDirectory"></param>
        public CBRProcessing(string fileDirectory)
        {
            SevenZipCompressor.SetLibraryPath("7z.dll");
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BitmapImage GetImage()
        {
            if (bitmap != null)
                return bitmap;
            else return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            ms.Dispose();
            sze.Dispose();
        }
    }
}
