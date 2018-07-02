using System;
using System.Collections.Generic;
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
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using DeepGo_CoreEngine;
using static DeepGo_CoreEngine.Core_Util;
using System.IO;

namespace DeepGo_App
{
    using GImg = Image<Gray, byte>;
    using CImg = Image<Bgr, byte>;

   
    /// <summary>
    /// Interaction logic for MainDisplay.xaml
    /// </summary>
    public partial class MainDisplay : UserControl
    {
        public event Action<List<string>> evtDropFiles;

        public MainDisplay()
        {
            InitializeComponent();
        }

        public void SetImage(string filepath)
        {
            ImgBack.Height = cvsMainDisplay.ActualHeight;
            ImgBack.Width = cvsMainDisplay.ActualWidth;
            ImgBack.Source = new CImg( filepath ).ToBitmapSource();
        }

        public void ResetImage()
        {
            ImgBack.Height = cvsMainDisplay.ActualHeight;
            ImgBack.Width = cvsMainDisplay.ActualWidth;
        }


        private void ImgBack_Drop( object sender, DragEventArgs e )
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop , false);
            var dirpath = files.First();
            if ( Directory.Exists( dirpath )  )
            {
                var pathlist = Directory.GetFiles(dirpath, "*.bmp")
                                .Union(Directory.GetFiles(dirpath, "*.jpg"))
                                .Union(Directory.GetFiles(dirpath, "*.png"))
                                .Union(Directory.GetFiles(dirpath, "*.jpeg"))
                                .OrderBy(x => System.IO.Path.GetFileName(x))
                                .ToList();

                evtDropFiles( pathlist );

            }
            else
            {
                
                MessageBox.Show( "Please select only one folder" );
                // path doesn't exist.

            }



            
        }
    }
}
