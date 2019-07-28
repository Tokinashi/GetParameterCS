using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetParameterCS
{
    public class DataSetting
    {
        public string InputDir { get; set; }
        public string OutputDir { get; set; }
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public Point WindowLocation { get; set; }
        public List<IDSetting> IDs { get; set; }

    }
    public class IDSetting
    {
        public string ID { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public IDSetting(string iD, int min, int max)
        {
            ID = iD;
            Min = min;
            Max = max;
        }
    }

}
