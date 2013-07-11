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
using ComicBookReader.Nomenclatures;

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
        private Point origin;
        private Point start;



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
                    if (Regex.Match
                        (s, Constants.Strings.RegExPattern, RegexOptions.IgnoreCase).
                        Success == true)
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
                image.Source = bitmap;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, ex.Message);
            }

            TransformGroup group = new TransformGroup();

            ScaleTransform xform = new ScaleTransform();
            group.Children.Add(xform);

            TranslateTransform tt = new TranslateTransform();
            group.Children.Add(tt);

            image.RenderTransform = group;

            image.MouseWheel += image_MouseWheel;
            image.MouseLeftButtonDown += image_MouseLeftButtonDown;
            image.MouseLeftButtonUp += image_MouseLeftButtonUp;
            image.MouseMove += image_MouseMove;
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!image.IsMouseCaptured) return;

            var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
            Vector v = start - e.GetPosition(Row1);            
            tt.X = origin.X - v.X;
            tt.Y = origin.Y - v.Y;
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image.CaptureMouse();
            var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
            start = e.GetPosition(Row1);
            origin = new Point(tt.X, tt.Y);
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TransformGroup transformGroup = (TransformGroup)image.RenderTransform;
            ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];
            double zoom = e.Delta > 0 ? 
                Constants.Image.ScaleImageStep : -Constants.Image.ScaleImageStep;
            if (transform.ScaleX > Constants.Image.MinimumScaleXYValue)
            {
                transform.ScaleX += zoom;
                transform.ScaleY += zoom;
            }

            if (transform.ScaleX < Constants.Image.MinimumScaleXYValue 
                && zoom > Constants.General.IntZero)
            {
                transform.ScaleX += zoom;
                transform.ScaleY += zoom;
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
                    image.Source = bitmap;
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
                    image.Source = bitmap;
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
                    if (Regex.Match
                        (s, Constants.Strings.RegExPattern, RegexOptions.IgnoreCase).Success == true)
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
                image.Source = bitmap;
            }
        }

        private void btnOpen_OnClick(object sender, RoutedEventArgs e)
        {
            sze = new SevenZipExtractor(getFileDirectory());
            archiveNames = new ReadOnlyCollection<string>(sze.ArchiveFileNames);            
            foreach (string s in archiveNames)
            {
                if (Regex.Match
                    (s, Constants.Strings.RegExPattern, RegexOptions.IgnoreCase).Success == true)
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
            image.Source = bitmap;
        }       
    }
}
