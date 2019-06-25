

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetParameterCS
{

    class ClsReadParameter
    {
        private string Fps { get;  }
        private string Csv { get;  }
        private string Gif { get;  }
        private List<string> Ids;
        private List<string> Min;
        private List<string> Max;

        public ClsReadParameter(string gifpath, string csvpath, string strfps)
        {
            this.Fps = strfps;
            Csv = csvpath;
            Gif = gifpath;
            Ids = new List<string>();
            Min = new List<string>();
        }
        public ClsReadParameter(string GifPath, string CsvPath)
        {
            Fps = "30.0";
            Csv = CsvPath;
            Gif = GifPath;
        }

        private void AddPosMinMax(List<string> Ids, List<string> Min, List<string> Max)
        {
            
        }
    }

}
