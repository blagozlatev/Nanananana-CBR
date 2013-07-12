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
    /// Class that manages all the operations and processes with the CB archive.
    /// </summary>
    class CBRProcessing:IDisposable
    {
        private int page;
        private SevenZipExtractor sze;        
        private Collection<string> mutableArchiveNames;
        private MemoryStream ms;
        private BitmapImage bitmap;
        
        /// <summary>
        /// A constructor for the class that gets the file directory
        /// and makes it ready for usage.
        /// </summary>
        /// <param name="fileDirectory"></param>
        public CBRProcessing(string fileDirectory)
        {
            SevenZipCompressor.SetLibraryPath(Constants.Strings.SevenZipLibrary);
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
        /// Gets the image for the current page in the CBA file.
        /// </summary>
        /// <returns>The image of the page.</returns>
        public BitmapImage GetImage()
        {
            if (bitmap != null)
                return bitmap;
            else return null;
        }


        /// <summary>
        /// Gets the next page in the CBA if there is a next page.
        /// </summary>
        /// <returns>The image of the page.</returns>
        public BitmapImage GetNext()
        {
            if (page < mutableArchiveNames.Count - Constants.General.IntOne)
                page++;
            ms.Dispose();
            ms = new MemoryStream();            
            sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
            ms.Seek(Constants.General.IntZero, SeekOrigin.Begin);                        
            return SetImage();
        }

        /// <summary>
        /// Gets the previous page in the CBA if there is a previous page.
        /// </summary>
        /// <returns>The image of the page.</returns>
        public BitmapImage GetPrevious()
        {
            if (page > Constants.General.IntZero)
                page--;
            ms.Dispose();
            ms = new MemoryStream();
            sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
            ms.Seek(Constants.General.IntZero, SeekOrigin.Begin);
            return SetImage();
        }

        /// <summary>
        /// Method that disposes the resources and frees all the memory used by the object.
        /// </summary>
        public void Dispose()
        {
            ms.Dispose();
            sze.Dispose();
        }
    }
}
