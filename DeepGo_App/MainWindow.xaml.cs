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
            PathList = files.Select( ( x, i ) => new GridData( i, x ) ).ToList();

            #region Display     
            cvsMainDisplay.SetImage( files.First() );
            cvsMainDisplay.Background = Brushes.Black;

            dtgMain.ItemsSource = PathList;
            dtgMain.Items.Refresh();

            cellno.Width   = DataGridLength.Auto;
            //cellname.Width = DataGridLength.Auto;
            #endregion
        }

        #region Window Event
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            cvsMainDisplay.evtDropFiles += SetDropFiles;
            ucmenuleft.evtBtn += btnClick;
        }

        private void Window_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            cvsMainDisplay.ResetImage();
        }

        private void btnClick( string name )
        {
            switch ( name )
            {
                case "btnLoad":
                    "".Print();
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

        #endregion


        #region DataGrid
        private void dtgMain_SelectedCellsChanged( object sender, SelectedCellsChangedEventArgs e )
        {
            var index = dtgMain.SelectedIndex.Print();
            if ( index >= 0 )
            {
                cvsMainDisplay.SetImage( PathList[index].path );
            }
        }
        #endregion
    }
}
