

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace GetParameterCS
{

    class ClsReadParameter
    {
        private enum Csv
        {
            Name,
            Id,
            Min,
            Mid,
            Max
        }
        private double Fps { get;  }
        private string Gif { get;  }
        private readonly List<string> ids;
        private readonly List<string> mins;
        private readonly List<string> maxs;
        private readonly List<Point> poses;
        private readonly List<Dictionary<double, double>> dicTimeVal;
        public double Duration { get; set; }
        public double AllValCnt { get; set; }
        public ClsReadParameter(string gifPath, string csvPath, double strfps)
        {
            Fps = strfps;
            Gif = gifPath;
            (ids, mins, maxs) = AddPosMinMax(csvPath);
            poses = SetPos(10);
            dicTimeVal = SetVals();
        }
        public ClsReadParameter(string gifPath, string csvPath)
        {
            Fps = 30.0;
            Gif = gifPath;
            (ids, mins, maxs) = AddPosMinMax(csvPath);
            poses = SetPos(10);
            dicTimeVal = SetVals();
        }

        private (List<string> Ids, List<string> Min, List<string> Max) AddPosMinMax(string csvpath)
        {
            string[] fields;
            string line;
            List<string> ids = new List<string>();
            List<string> min = new List<string>();
            List<string> max = new List<string>();

            StreamReader sr = new StreamReader(csvpath);
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    fields = line.Split(',');
                    ids.Add(fields[(int)Csv.Id]);
                    min.Add(fields[(int)Csv.Min]);
                    max.Add(fields[(int)Csv.Max]);
                }

                return (ids, min, max);
            }
        }

        private List<Point> SetPos(int swPoint = 10)
        {
            List<Point> pos = new List<Point>();
            
            Image image = Image.FromFile(Gif);
            double gifheight = (double)image.Height;
            Double sq = gifheight / swPoint;
            List<Point> point = new List<Point>();
            int i, row, col;

            // 左上が起点として高さのswPoint割り
            for  (i = 0,col = -1, row = 0; i < ids.Count; i++)
            {
                if ((i % swPoint) == 0)
                {
                    row = 0;
                    ++col;
                }
                pos.Add(new Point((int)((sq / 2) + sq * col), (int)((sq / 2) + sq * row)));
                ++row;
            }

            return pos;
        }

        private List<Dictionary<double, double>> SetVals()
        {
            List<Dictionary<double, double>> listval = new List<Dictionary<double, double>>();
            Image image = Image.FromFile(Gif);
            FrameDimension fd = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCnt = image.GetFrameCount(fd);
            Duration = FtoTime(frameCnt - 1);
            Color gifColor;
            double preval = 0,compareval = 0;
            for  (int framei  = 0; framei < ids.Count; framei++){

                listval.Add(new Dictionary<double, double>());
            }
            for (int frameInd = 0;frameInd < frameCnt;frameInd++) 
            {
                image.SelectActiveFrame(fd, frameInd);
                Bitmap bitmap = (Bitmap)image;
                for (int idInd = 0; idInd < ids.Count; idInd++)
                {
                    gifColor = bitmap.GetPixel(poses[idInd].X, poses[idInd].Y);
                    compareval = 1.0 - gifColor.GetBrightness();
                    if (frameInd == 0 || preval != compareval || frameInd == (ids.Count-1))
                    {
                        listval[idInd].Add(FtoTime(frameInd),compareval);
                    }
                    preval = compareval;
                }
            }
            AllValCnt = 0;
            foreach (Dictionary<double,double> dicVal in listval)
            {
                AllValCnt += dicVal.Count;
            } 

            return listval;
        }

        private double ChgVal(double value,double min,double max)
        {
            double distance = max - min;
            return (distance * value) + min;
        }

        private double FtoTime(int flame)
        {
            double ans = (1.0 / Fps) * flame;
            return ans;
        }
    }

}
