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
        public string IDPerson;
        public string IDDoc;
        public string ImgPath;
        public int BarcodeXPerson;
        public int BarcodeYPerson;
        public int BarcodeXDoc;
        public int BarcodeYDoc;
        public List<BoxInfo> BoxInfoList;
    }

    public struct BoxInfo
    {
        public int x0;
        public int y0;
        public int x1;
        public int y1;
        public int w { get { return x1 - x0; }  set { } }
        public int h { get { return y1 - y0; }  set { } }

        public string type;
        public string content;

        public BoxInfo( int x0, int y0, int x1, int y1, string type, string content )
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
            this.type = type;
            this.content = content;
        }
    }

    public static class Ext
    {
        public static List<ResDocData> ResultToDataClass( this string resultPath )
        {
            string[][] res = ReadCsv2String(resultPath , rowskip: 1 );
            res.Act( x => x.Print() );

            return null;
        }


    }



}
