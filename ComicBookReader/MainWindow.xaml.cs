using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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

namespace ComicBookReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ZipArchive zip = ZipFile.OpenRead("Hellblazer 001.cbz");
            ZipArchiveEntry entry1 = zip.GetEntry("P00001.jpg");            
            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = entry1.Open();
            imageSource.EndInit();
            Page.Source = imageSource;
        }
    }
}
