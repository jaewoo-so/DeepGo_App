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
using SpeedyCoding;
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
        public ResDocData SelectedDoc;

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


        #region Create Modify event and funtion
        public void DrawBtn( ResDocData infos ) 
        {
            // Set Current Doc
            SelectedDoc = infos;

            cvsMainDisplay.Children.Clear();
            int posNum = infos.BoxInfoList.Count;
            Button[] btn = new Button[posNum];

            for ( int i = 0 ; i < posNum ; i++ )
            {
                var box = infos.BoxInfoList[i];
                var w = box.x1 - box.x0;
                var h = box.y1 - box.y0;

                // Bind event
                var newbtn = CheckButton( i , w , h );
                Canvas.SetLeft( newbtn, box.x0 );
                Canvas.SetTop( newbtn, box.y0 );

                cvsMainDisplay.Children.Add( newbtn );
                btn[i] = newbtn;
            }
        }

        private Button CheckButton( int i , int w , int h ) 
        {
            var btn = new Button();
            btn.Name = "btn_" + i.ToString();
            btn.Width = w;
            btn.Height = h;
            btn.Opacity = 0.9;
            btn.Background = Brushes.LawnGreen;
            btn.Click += ClickIdx;
            return btn;
        }

        public void ClickIdx( object sender, RoutedEventArgs e ) 
        {
            try
            {
                var self = sender as Button;
                var name = self.Name;
                var idx = name.Split('_').Last().Split('.').First();
                this.IsEnabled = false;
                Win_ModifyNum mnum = new Win_ModifyNum();
                mnum.ShowDialog();
            }
            catch ( Exception ex )
            { ex.Print( "Map Click Error Msg " ); }

        }

        #endregion  



    }
}
