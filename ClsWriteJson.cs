using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GetParameterCS
{
    class ClsWriteJson
    {

        private Live2dJson live2d;
        public ClsWriteJson(
                   double Duration ,
                   double Fps,
                   long CurveCount ,
                   bool @loop  = true,
                   bool AreBeziersRestricted  = true,
                   int UserDataCount  = 0,
                   int TotalUserDataSize  = 0)
        {
            live2d = new Live2dJson
            {
                Curves = new List<CurvePoint>(),
                Meta = new Dictionary<string, object>
            {
                { "Duration", Calc(Duration) },
                { "Fps", Fps },
                { "Loop", @loop },
                { "AreBeziersRestricted", AreBeziersRestricted },
                { "CurveCount", CurveCount },
                { "TotalSegmentCount", 0 },
                { "TotalPointCount", 0 },
                { "UserDataCount", UserDataCount },
                { "TotalUserDataSize", TotalUserDataSize }
            }
            };
        }

        public void WriteJson(string jsonPath)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            string output = JsonConvert.SerializeObject(live2d, settings);
            output = output.Replace("  ", "\t");
            using (StreamWriter sw = new System.IO.StreamWriter(jsonPath))
            {
                string[] rn = { "\r\n" };
                string[] line = output.Split(rn,StringSplitOptions.None);
                string printline;
                foreach (string str in line)
                {
                    printline = str.Replace("0.0,", "0,");
                    sw.WriteLine(printline);
                    
                }
                sw.Close();
            }
        }

        public void AddPoint(string id, List<double> keys, List<double> value)
        {
            CurvePoint newSeg = new CurvePoint
            {
                Id = id,
                Segments = new List<double>()
            };
            for (int i = 0; i < keys.Count; i++)
            {
                newSeg.Segments.Add(Calc(keys[i]));
                live2d.Meta["TotalPointCount"] = (int)live2d.Meta["TotalPointCount"] + 1;
                newSeg.Segments.Add(Calc(value[i]));
                live2d.Meta["TotalPointCount"] = (int)live2d.Meta["TotalPointCount"] + 1;

                if (i != (keys.Count - 1))
                {
                    newSeg.Segments.Add(0);
                    live2d.Meta["TotalSegmentCount"] = (int)live2d.Meta["TotalPointCount"] + 1;
                }

            }
            live2d.Curves.Add(newSeg);

        }
        private double Calc(double num)
        {
            return Math.Truncate(num * 100.0) / 100.0;
        }
    }
    


    class Live2dJson
    {
        public double Version = 3.0;
        public Dictionary<string, object> Meta { get; set; }
        public List<CurvePoint> Curves { get; set; }
    }
    class CurvePoint
    {
        public string Target = "Parameter";
        public string Id { get; set; }
        public List<double> Segments { get; set; }
    }
}
