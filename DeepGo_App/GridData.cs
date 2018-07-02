using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGo_App
{
    public class GridData : INotifyPropertyChanged
    {
        int _no { get; set; }
        string _name { get; set; }

        public int no { get { return _no; } set { _no = value; Notify( "no" ); } }
        public string name { get { return _name; } set { _name = value; Notify( "name" ); } }
        public string path;

        public GridData( int num, string path )
        {
            this.no = num;
            this.path = path;
            this.name = Path.GetFileName( path );
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify( string propName )

        {
            if ( this.PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propName ) );
            }
        }

    }
}
