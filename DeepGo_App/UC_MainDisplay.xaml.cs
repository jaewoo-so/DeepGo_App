using DeepGo_CoreEngine;
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
using SpeedyCoding;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DeepGo_App
{
    /// <summary>
    /// Interaction logic for UC_MainDisplay.xaml
    /// </summary>
    public partial class UC_MainDisplay : UserControl
    {
        public ResDocData SelectedDoc;

        public UC_MainDisplay()
        {
            InitializeComponent();
        }



        public void SetImage( string srcpath , ResDocData dataInfo ) // done 
        {
            // 원래 이미지 인풋이였는데 string 패쓰 인풋해준다 
            // data인포로 그려주는데, 좌표를 리 스케일링 해서 비율 맞춰야 한다. 

            var cvsw = cvsMap.ActualWidth;
            var cvsh = cvsMap.ActualHeight;
            var img = new Image<Bgr,byte>(srcpath);
            var imgw = img.Width;
            var imgh = img.Height;

            imgMap.ImageSource = img.ToBitmapSource();


            DrawBtn( dataInfo, cvsw / imgw, cvsh / imgh );


        }

        public void test( ResDocData dataInfo  , double wratio , double hratio)
        {
            cvsMap.Children.Clear();

            var box = dataInfo.BoxInfoList.First();
            int w = (int)(box.w*wratio);
            int h = (int)(box.h*hratio);

            // Bind event
            var newbtn = CheckButton( 0 , w , h , dataInfo.BoxInfoList.First().content );
            Canvas.SetLeft( newbtn, box.x0 *wratio);
            //Canvas.SetLeft( newbtn, 200 );
            Canvas.SetTop( newbtn, box.y0 *hratio+h);
            //Canvas.SetTop( newbtn, 200 );

            cvsMap.Children.Add( newbtn );
        }


        #region Create Modify event and funtion
        public void DrawBtn( ResDocData infos, double wratio, double hratio )
        {
            // Set Current Doc
            SelectedDoc = infos;

            cvsMap.Children.Clear();
            int posNum = infos.BoxInfoList.Count;
            Button[] btn = new Button[posNum];

            for ( int i = 0 ; i < posNum ; i++ )
            {
                var box = infos.BoxInfoList[i];
                int w = (int)(box.w*wratio);
                int h = (int)(box.h*hratio);

                // Bind event
                var newbtn = CheckButton( i , w , h , box.content );
                Canvas.SetLeft( newbtn, box.x0 * wratio );
                Canvas.SetTop( newbtn, box.y0 * hratio + h );

                cvsMap.Children.Add( newbtn );
                btn[i] = newbtn;
            }
        }

        private Button CheckButton( int i, int w, int h , string content)
        {
            var btn = new Button();
            btn.Name = "btn_" + i.ToString();
            btn.Width = w;
            btn.Height = h;
            //btn.Opacity = 0.9;
            btn.Background = (SolidColorBrush)( new BrushConverter().ConvertFrom( "#505050" ) );
            btn.Foreground = Brushes.White;
            btn.Content = content;

            btn.FontSize = (int)(w * 0.6);
            btn.FontWeight = FontWeights.DemiBold;
            btn.VerticalContentAlignment = VerticalAlignment.Center;
            btn.Padding = new Thickness( 1, 1, 1, 1 );
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
