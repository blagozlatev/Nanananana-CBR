using SharpCompress.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SevenZip;
using System.Runtime.InteropServices;

namespace ComicBookReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        int page = 0;
        static ZipArchive zip = ZipFile.OpenRead("Hellblazer 001.cbz");
        List<ZipArchiveEntry> entries = zip.Entries.ToList();
        ZipArchiveEntry entry;
        
        public MainWindow()
        {
            InitializeComponent();
            SevenZipCompressor.SetLibraryPath("7z.dll");            
            SevenZipExtractor sze = new SevenZipExtractor("Batman_Beyond_025_282013_29_28digital_29_28Son_of_Ultron_Empire_29.cbr");                        
            MemoryStream ms = new MemoryStream();
            //sze.BeginExtractFile(1, ms);            
            sze.ExtractFile(1, ms);                        
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            Page.Source = bitmap;                       
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            foreach (ZipArchiveEntry z in entries)
            {
                string str = string.Empty;
                if (e.Key == Key.Right)
                {                    
                    str = "0" + page;
                    if (Regex.Match(z.Name, str).Success == true)
                    {
                        entry = z;
                        var imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = entry.Open();
                        imageSource.EndInit();
                        Page.Source = imageSource;
                        page++;
                        break;
                    }
                }
                else if (e.Key == Key.Left)
                {                    
                    str = "0" + page;
                    if (Regex.Match(z.Name, str).Success == true)
                    {
                        entry = z;
                        var imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = entry.Open();
                        imageSource.EndInit();
                        Page.Source = imageSource;
                        if (page != 1)
                        {
                            page--;
                        }
                        break;
                    }
                }
            }
            */

        }
    }
}
