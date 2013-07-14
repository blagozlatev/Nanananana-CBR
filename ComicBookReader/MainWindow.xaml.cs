using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SevenZip;
using Microsoft.Win32;
using System.IO;
using System.Collections.Generic;

namespace NananananaCBR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {        
        private Point origin;
        private Point start;
        CBRProcessing cbr;
        DirectoryInfo dir;

        public MainWindow()
        {
            InitializeComponent();                                    
            try
            {
                cbr = new CBRProcessing(getFileDirectory());
                image.Source = cbr.GetImage();
            }
            catch (ArgumentNullException) { }
            dir = new DirectoryInfo(@"E:\Downloads\Hellblazer");
            List<FileInfo> fi = dir.GetFiles().ToList();            
            foreach (FileInfo f in fi)
            {                
                library.Items.Add(f.Name);
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
            double zoom = e.Delta > Constants.General.IntZero ? 
                Constants.Image.ScaleImageStep : -Constants.Image.ScaleImageStep;
            if (transform.ScaleX >= Constants.General.IntOne)
            {
                transform.ScaleX += zoom;
                transform.ScaleY += zoom;
            } else if (transform.ScaleX >= Constants.Image.MinimumScaleXYValue
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
            openFile.DefaultExt = Constants.Strings.DefaultOpenDialogExtension;
            openFile.Filter = Constants.Strings.OpenDialogFilter;
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
                if (cbr != null)
                    image.Source = cbr.GetNext();                
            }
            if (e.Key == Key.Left)
            {
                if (cbr != null)
                    image.Source = cbr.GetPrevious();
            }
        }

        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || (Keyboard.IsKeyDown(Key.RightCtrl)))
                && Keyboard.IsKeyDown(Key.O))
            {
                try
                {
                    if (cbr != null)
                        cbr = new CBRProcessing(getFileDirectory());
                    image.Source = cbr.GetImage();
                }
                catch (ArgumentNullException) { }
            }
        }

        private void btnOpen_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbr != null)
                    cbr = new CBRProcessing(getFileDirectory());
                image.Source = cbr.GetImage();
            }
            catch (ArgumentNullException) { }
        }

        private void btnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            if (cbr != null)
                image.Source = cbr.GetPrevious();
        }

        private void btnNext_OnClick(object sender, RoutedEventArgs e)
        {
            if (cbr != null)
                image.Source = cbr.GetNext();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new About().Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void library_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {            
            cbr = new CBRProcessing(dir.FullName + "\\" + library.SelectedItem.ToString());
            image.Source = cbr.GetImage();
        }

        private void library_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cbr = new CBRProcessing(dir.FullName + "\\" + library.SelectedItem.ToString());
                image.Source = cbr.GetImage();
            }
        }        
    }
}