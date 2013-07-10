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
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace ComicBookReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    

    public partial class MainWindow : Window
    {
        int page = 0;
        SevenZipExtractor sze;
        ReadOnlyCollection<string> archiveNames;
        Collection<string> mutableArchiveNames = new Collection<string>();
        MemoryStream ms;
        BitmapImage bitmap;

        public MainWindow()
        {
            InitializeComponent();
            SevenZipCompressor.SetLibraryPath("7z.dll");
            try
            {                
                sze = new SevenZipExtractor(getFileDirectory());
                archiveNames = sze.ArchiveFileNames;
                foreach (string s in archiveNames)
                {
                    if (Regex.Match(s, "d*.png|d*.jpg|d*.jpeg|d*.tiff|d*.gif").Success == true)
                    {
                        mutableArchiveNames.Add(s);
                    }
                }
                archiveNames = null;
                ms = new MemoryStream();
                sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                Page.Source = bitmap;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, ex.Message);
            }
        }
        
        private string getFileDirectory()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.FileName = string.Empty;
            openFile.DefaultExt = ".cbr";
            openFile.Filter = "Comic Book Archive (*.cbr, *.cbz, *.cba)|*.cbr;*.cbz;*.cba";
            var fileFound = openFile.ShowDialog();
            if (fileFound == true)
            {
                return openFile.FileName;
            }
            return null;
        }       

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {      
            if (e.Key == Key.Right)
            {
                if (ms != null && sze != null)
                {
                    if (page < mutableArchiveNames.Count-1)
                        page++;
                    ms.Dispose();
                    ms = new MemoryStream();
                    sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    ms.Seek(0, SeekOrigin.Begin);
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    Page.Source = bitmap;
                }
            }
            if (e.Key == Key.Left)
            {
                if (ms != null && sze != null)
                {
                    if (page > 0)
                        page--;                    
                    ms.Dispose();
                    ms = new MemoryStream();
                    sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    ms.Seek(0, SeekOrigin.Begin);
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    Page.Source = bitmap;
                }
            }
        }

        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || (Keyboard.IsKeyDown(Key.RightCtrl)))
                && Keyboard.IsKeyDown(Key.O))
            {
                sze = new SevenZipExtractor(getFileDirectory());
                archiveNames = sze.ArchiveFileNames;
                foreach (string s in archiveNames)
                {
                    if (Regex.Match(s, "d*.png|d*.jpg|d*.jpeg|d*.tiff|d*.gif").Success == true)
                    {
                        mutableArchiveNames.Add(s);
                    }
                }
                archiveNames = null;
                ms = new MemoryStream();
                sze.ExtractFile(mutableArchiveNames.ElementAt(page), ms);
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                Page.Source = bitmap;
            }
        }
    }
}
