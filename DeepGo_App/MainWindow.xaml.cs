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
using MahApps.Metro.Controls;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using SpeedyCoding;

#region
using DeepGo_CoreEngine;
using static DeepGo_CoreEngine.CoreData;
using static DeepGo_CoreEngine.Core_Util;
using System.Windows.Forms;
using System.IO;

#endregion

namespace DeepGo_App
{
    using GImg = Image<Gray, byte>;
    using CImg = Image<Bgr, byte>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        List<GridData> PathList;
        List<ResDocData> DataInfoAll;
        ResDocData SelectedDoc;

        public MainWindow()
        {
            InitializeComponent();
            PathList = new List<GridData>();
           
        }

        public void ResetWindow()
        {
            PathList = null;
            dtgMain.ItemsSource = PathList;
            dtgMain.Items.Refresh();

        }

        public void SetDropFiles(List<string> files)
        {
            //global
            PathList = files.Select( ( x, i ) => new GridData( i, x ) ).ToList();
            DisplayPathImg();
        }

        public void SetLoadFile( List<ResDocData> dataInfoAll )
        {
            //global
            PathList = dataInfoAll.ToGridDataList();
            DisplayPathImg();
            //test( dataInfoAll.First() );
            //CreateSecondBtn( dataInfoAll.First() );
        }

       
        //public void test( ResDocData dataInfo )
        //{
        //    cvsMainDisplay.Children.Clear();

        //    var box = dataInfo.BoxInfoList.First();
        //    var w = box.w;
        //    var h = box.h;

        //    // Bind event
        //    var newbtn = CheckButton( 0 , w , h );
        //    Canvas.SetLeft( newbtn, box.x0 );
        //    Canvas.SetTop( newbtn, box.y0 );

        //    cvsMainDisplay.Children.Add( newbtn );
        //}
        
        public void CreateSecondBtn( ResDocData dataInfo )
        {
            //DrawBtn( dataInfo );
        }


        public void DisplayPathImg()
        {
            //SetImage( PathList.First().path );
            ucMainDisply.SetImage(PathList.First().path , DataInfoAll.First() );
            //Background = Brushes.Black;

            dtgMain.ItemsSource = PathList;
            dtgMain.Items.Refresh();

            cellno.Width = DataGridLength.Auto;
        }


        #region Window Event
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            ucmenuleft.evtBtn += btnClick;
        }

        private void Window_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            ResetImage();
        }

        private void btnClick( string name )
        {
            switch ( name )
            {
                case "btnLoad":
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = @"F:\00_github_nchos\DeepGo_App\data";
                    if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                    {
                        //global
                        DataInfoAll = ofd.FileName.ResultToDataClass();
                        SetLoadFile( DataInfoAll );
                    }
                   
                    // Create Second Box 


                    break;

                case "btnReset":
                    Console.WriteLine();
                    break;

                case "btnSave":
                    break;

                case "btnHide":
                    break;
            }
        }


        private void ImgBack_Drop( object sender, System.Windows.DragEventArgs e )
        {
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop , false);
            var dirpath = files.First();
            if ( Directory.Exists( dirpath ) )
            {
                var pathlist = Directory.GetFiles(dirpath, "*.bmp")
                                .Union(Directory.GetFiles(dirpath, "*.jpg"))
                                .Union(Directory.GetFiles(dirpath, "*.png"))
                                .Union(Directory.GetFiles(dirpath, "*.jpeg"))
                                .OrderBy(x => System.IO.Path.GetFileName(x))
                                .ToList();

                SetDropFiles( pathlist );

            }
            else
            {

                System.Windows.Forms.MessageBox.Show( "Please select only one folder" );
                // path doesn't exist.

            }
        }

        #endregion

        #region Main Display
        public void SetImage( string filepath )
        {
            //ImgBack.Height = cvsMainDisplay.ActualHeight;
            //ImgBack.Width = cvsMainDisplay.ActualWidth;
            //ImgBack.ImageSource = new CImg( filepath ).ToBitmapSource();

        }

        public void ResetImage()
        {
            //ImgBack.Height = cvsMainDisplay.ActualHeight;
            //ImgBack.Width = cvsMainDisplay.ActualWidth;
        }




        #region Create Modify event and funtion
        //public void DrawBtn( ResDocData infos )
        //{
        //    // Set Current Doc
        //    SelectedDoc = infos;

        //    cvsMainDisplay.Children.Clear();
        //    int posNum = infos.BoxInfoList.Count;
        //    System.Windows.Controls.Button[] btn = new System.Windows.Controls.Button[posNum];

        //    for ( int i = 0 ; i < posNum ; i++ )
        //    {
        //        var box = infos.BoxInfoList[i];
        //        var w = box.x1 - box.x0;
        //        var h = box.y1 - box.y0;

        //        // Bind event
        //        var newbtn = CheckButton( i , w , h );
        //        Canvas.SetLeft( newbtn, box.x0 );
        //        Canvas.SetTop( newbtn, box.y0 );

        //        cvsMainDisplay.Children.Add( newbtn );
        //        btn[i] = newbtn;
        //    }
        //}

        private System.Windows.Controls.Button CheckButton( int i, int w, int h )
        {
            var btn = new System.Windows.Controls.Button();
            btn.Name = "btn_" + i.ToString();
            btn.Width = w;
            btn.Height = h;
            btn.Opacity = 1.0;
            btn.Background = Brushes.LawnGreen;
            btn.Click += ClickIdx;
            return btn;
        }

        public void ClickIdx( object sender, RoutedEventArgs e )
        {
            try
            {
                var self = sender as System.Windows.Controls.Button;
                var name = self.Name;
                var idx = name.Split('_').Last().Split('.').First();
                //this.IsEnabled = false;
                Win_ModifyNum mnum = new Win_ModifyNum();
                mnum.ShowDialog();
            }
            catch ( Exception ex )
            { ex.Print( "Map Click Error Msg " ); }

        }

        #endregion  
        #endregion



        #region DataGrid
        private void dtgMain_SelectedCellsChanged( object sender, SelectedCellsChangedEventArgs e )
        {
            var index = dtgMain.SelectedIndex;
            if ( index >= 0 )
            {
                ucMainDisply.SetImage(PathList[index].path , DataInfoAll[index] );
            }
        }
        #endregion
    }
}
