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

namespace DeepGo_App
{
    /// <summary>
    /// Interaction logic for MenuLeft.xaml
    /// </summary>

    public delegate void BtnEvt( string name );

    public partial class MenuLeft : UserControl
    {
        public event BtnEvt evtBtn;
        public MenuLeft()
        {
            InitializeComponent();
        }

        private void btnClick( object sender, RoutedEventArgs e )
            => evtBtn( ( sender as Button ).Name );
    }
}
