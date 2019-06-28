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
                   Double Duration ,
                   Double Fps,
                   int CurveCount ,
                   bool @loop  = true,
                   bool AreBeziersRestricted  = true,
                   int UserDataCount  = 0,
                   int TotalUserDataSize  = 0)
        {
            live2d = new Live2dJson();
            live2d.Meta.Add("Duration", Duration);
            live2d.Meta.Add("Fps", Fps);
            live2d.Meta.Add("Loop", @loop);
            live2d.Meta.Add("AreBeziersRestricted", AreBeziersRestricted);
            live2d.Meta.Add("CurveCount", CurveCount);
            live2d.Meta.Add("TotalSegmentCount", 0);
            live2d.Meta.Add("TotalPointCount", 0);
            live2d.Meta.Add("UserDataCount", UserDataCount);
            live2d.Meta.Add("TotalUserDataSize", TotalUserDataSize);
        }

        public void WriteJson(string jsonName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            string output = JsonConvert.SerializeObject(live2d, settings);
            output = output.Replace("  ", "\t");
            using (StreamWriter sw = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + jsonName + ".motion3.json"))
            {
                string[] rn = { "\r\n" };
                string[] line = output.Split(rn,StringSplitOptions.None);
                string printline;
                foreach (string str in line)
                {
                    printline = str.Replace("  ", "\r\n");
                    sw.WriteLine(printline);
                }
                sw.Close();
            }
        }

        private void AddPoint(string id, List<double> keys , List<double> value)
        {
            CurvePoint newSeg = new CurvePoint
            {
                Id = id
            };
            for (int i = 0; i < keys.Count; i++)
            {
                newSeg.Segments.Add(keys[i]);
                live2d.Meta["TotalPointCount"] = (int)live2d.Meta["TotalPointCount"] + 1;
                newSeg.Segments.Add(value[i]);
                live2d.Meta["TotalPointCount"] = (int)live2d.Meta["TotalPointCount"] + 1;

                if (i != (keys.Count - 1))
                {
                    newSeg.Segments.Add(0);
                    live2d.Meta["TotalSegmentCount"] = (int)live2d.Meta["TotalPointCount"] + 1;
                }

            }
            live2d.Curves.Add(newSeg);

        }
    }
    

    class Live2dJson
    {
        public int Version = 3;
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
