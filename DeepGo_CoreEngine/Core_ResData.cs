using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using static ApplicationUtilTool.FileIO.CsvTool;


namespace DeepGo_CoreEngine
{

    public class ResDocData
    {
        public string       IDPerson         ;
        public string       IDDoc            ;
        public string       ImgPath          ;
        public int          BarcodeXPerson    ;
        public int          BarcodeYPerson    ;
        public int          BarcodeXDoc       ;
        public int          BarcodeYDoc       ;
        public List<BoxInfo> BoxInfoList       ;

        public ResDocData( string[] data, List<BoxInfo> infolist )
        {
            IDPerson = data[0];
            IDDoc = data[1];
            ImgPath = data[2];
            BarcodeXPerson = int.Parse( data[3] );
            BarcodeYPerson = int.Parse( data[4] );
            BarcodeXDoc = int.Parse( data[5] );
            BarcodeYDoc = int.Parse( data[6] );
            BoxInfoList = infolist;
        }

        public ResDocData( string pid , string idd, string imgpath, int barpx, int barpy, int bardx, int bardy, List<BoxInfo> infolist )
        {
            IDPerson = pid;
            IDDoc = idd;
            ImgPath = imgpath;
            BarcodeXPerson = barpx;
            BarcodeYPerson = barpy;
            BarcodeXDoc = bardx;
            BarcodeYDoc = bardy;
            BoxInfoList = infolist;
        }
    }

    public struct BoxInfo
    {
        public int x0;
        public int y0;
        public int x1;
        public int y1;
        public int w { get { return x1 - x0; } set { } }
        public int h { get { return y1 - y0; } set { } }

        public string type;
        public string content;

        public BoxInfo( int x0, int x1, int y0, int y1, string type, string content )
        {
            this.x0 = x0;
            this.x1 = x1;
            this.y0 = y0;
            this.y1 = y1;
            this.type = type;
            this.content = content;
        }

        public BoxInfo( string[] list )
        {
            this.x0 = int.Parse( list[0] );
            this.x1 = int.Parse( list[1] );
            this.y0 = int.Parse( list[2] );
            this.y1 = int.Parse( list[3] );
            this.type = list[4];
            this.content = list[5];
        }
    }

    public static class Ext
    {
        public static List<ResDocData> ResultToDataClass( this string resultPath )
        {
            List<ResDocData> resuletlist = new List<ResDocData>();

            // Read Csv File
            string[][] res = ReadCsv2String(resultPath , rowskip: 1 , order0Dirction : false);
            
            // Group by Personal ID
            var grouped = res.GroupBy(x => x[1] )
                           .Select(x => new { key = x.Key , data = x.ToArray() } ).ToList();
            
            foreach ( var docdata in grouped )
            {
                string[] infolist = docdata.data.First().Take(7).ToArray();
                var boxinfolist = docdata.data.Select(x => x.Skip(7).ToArray()).ToArray().ToBoxInfo();
                ResDocData output = new ResDocData(infolist ,boxinfolist );
                resuletlist.Add( output );
            }
                

            return resuletlist;
        }

        public static List<BoxInfo> ToBoxInfo( this string[][] datalist )
            => datalist.Select( x => new BoxInfo(x) ).ToList();
            


    }



}
