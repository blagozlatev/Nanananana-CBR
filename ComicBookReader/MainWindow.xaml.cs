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
            //rotateImage.Angle = Constants.Image.NullRotation;
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
            Vector v = start - e.GetPosition(border);
            tt.X = origin.X - v.X;
            tt.Y = origin.Y - v.Y;
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image.CaptureMouse();
            var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
            start = e.GetPosition(border);
            origin = new Point(tt.X, tt.Y);
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TransformGroup transformGroup = (TransformGroup)image.RenderTransform;
            ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

            double zoom = e.Delta > 0 ? .2 : -.2;
            transform.ScaleX += zoom;
            transform.ScaleY += zoom;
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
                image.Source = bitmap;
            }
        }

        private void btnZoomIn_OnClick(object sender, RoutedEventArgs e)
        {
            //scaleImage.ScaleX += Constants.Image.ScaleImageStep;
            //scaleImage.ScaleY += Constants.Image.ScaleImageStep;
        }

        private void btnZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            //if (scaleImage.ScaleX >= 0.3)
            //{
            //    scaleImage.ScaleX -= Constants.Image.ScaleImageStep;
            //    scaleImage.ScaleY -= Constants.Image.ScaleImageStep;
            //}
        }

        private void btnFillWindow_OnClick(object sender, RoutedEventArgs e)
        {
            //scaleImage.ScaleX = Constants.General.IntOne;
            //scaleImage.ScaleY = Constants.General.IntOne;
            //image.Width = scrlVwrForImage.ActualWidth - 10;
            //image.Height = scrlVwrForImage.ActualHeight - 10;
        }

        private void btnOriginalSize_OnClick(object sender, RoutedEventArgs e)
        {
            //BitmapImage bmp = image.Source as BitmapImage;
            //image.Height = bmp.Height;
            //image.Width = bmp.Width;
            //scaleImage.ScaleX = Constants.General.IntOne;
            //scaleImage.ScaleY = Constants.General.IntOne;
        }

        private void txtCustomZoom_OnEnter(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    double customZoomPercentage = Constants.General.IntZero;
            //    if (double.TryParse(txtCustomZoom.Text, out customZoomPercentage))
            //    {
            //        customZoomPercentage = double.Parse(txtCustomZoom.Text);
            //    }
            //    if (customZoomPercentage >= Constants.Image.MinimumZoomPercentage
            //        && customZoomPercentage <= Constants.Image.MaximumZoomPercentage)
            //    {
            //        double zoomRatio = Constants.General.IntOne *
            //            (customZoomPercentage / Constants.General.IntHundred);
            //        scaleImage.ScaleX = zoomRatio;
            //        scaleImage.ScaleY = zoomRatio;
            //    }
            //    else
            //    {
            //        MessageBox.Show("The value for custom zoom must be between 30 and 400!", "Error!");
            //    }
            //}
        }

        private void btnBrowse_OnClick(object sender, RoutedEventArgs e)
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
            image.Source = bitmap;
        }

        private void btnRotateLeft_OnClick(object sender, RoutedEventArgs e)
        {/*
            rotateImage.Angle -= Constants.Image.AngleRotation;
            if (rotateImage.Angle == -Constants.Image.FullRotation)
            {
                rotateImage.Angle = Constants.Image.NullRotation;
            }
            scaleImage.CenterX = image.ActualWidth / Constants.Image.DivisorForCenterOfImage;
            scaleImage.CenterY = image.ActualHeight / Constants.Image.DivisorForCenterOfImage;
          */
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {            
            //image.Width = scrlVwrForImage.ActualWidth - 10;
            //image.Height = scrlVwrForImage.ActualHeight - 10;
            //scaleImage.ScaleX = Constants.General.IntOne;
            //scaleImage.ScaleY = Constants.General.IntOne;

            //scaleImage.CenterX = image.ActualWidth / Constants.Image.DivisorForCenterOfImage;
            //scaleImage.CenterY = image.ActualHeight / Constants.Image.DivisorForCenterOfImage;
            //scaleImage.ScaleX = Constants.General.IntOne;
            //scaleImage.ScaleY = Constants.General.IntOne;          
        }    
    }
}
