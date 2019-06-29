using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GetParameterCS
{
    /// <summary>
    /// パラメータを読み取り格納するクラス
    /// </summary>
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
        public readonly List<string> ids;
        private readonly List<double> mins;
        private readonly List<double> maxs;
        private readonly List<Point> poses;
        public readonly List<Dictionary<double, double>> dicTimeVal;
        public double Duration { get; set; }
        public long AllValCnt { get; set; }

        public int GetCount()
        {
            return ids.Count;
        }

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

        private (List<string> Ids, List<double> Min, List<double> Max) AddPosMinMax(string csvpath)
        {
            string[] fields;
            string line;
            List<string> ids = new List<string>();
            List<double> min = new List<double>();
            List<double> max = new List<double>();
            string strmin;
            string strmax;

            StreamReader sr = new StreamReader(csvpath);
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    fields = line.Split(',');
                    ids.Add(fields[(int)Csv.Id]);
                    strmin = fields[(int)Csv.Min];
                    strmax = fields[(int)Csv.Max];
                    min.Add(int.Parse(strmin));
                    max.Add(int.Parse(strmax));
                }

                return (ids, min, max);
            }
        }

        /// <summary>
        /// ID別の位置リスト
        /// </summary>
        /// <param name="swPoint">縦の個数</param>
        /// <returns></returns>
        private List<Point> SetPos(int swPoint = 10)
        {
            List<Point> pos = new List<Point>();
            
            Image image = Image.FromFile(Gif);
            double gifheight = image.Height;
            double sq = gifheight / swPoint;
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

        /// <summary>
        /// 時刻とパラメータ量の辞書をIDごとにリスト化して格納
        /// </summary>
        /// <returns>辞書(時刻、パラメータ量)のリスト</returns>
        private List<Dictionary<double, double>> SetVals()
        {
            List<Dictionary<double, double>> listval = new List<Dictionary<double, double>>();
            Image image = Image.FromFile(Gif);
            FrameDimension fd = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCnt = image.GetFrameCount(fd);
            Duration = FtoTime(frameCnt - 1);
            Color gifColor;
            double preval = 0;
            for  (int framei  = 0; framei < ids.Count; framei++){

                listval.Add(new Dictionary<double, double>());
            }
            for (int frameInd = 0;frameInd < frameCnt;frameInd++) 
            {
                image.SelectActiveFrame(fd, frameInd);
                Bitmap bitmap = (Bitmap)image;
                for (int idInd = 0; idInd < ids.Count; idInd++)
                {
                    double compareval;
                    gifColor = bitmap.GetPixel(poses[idInd].X, poses[idInd].Y);
                    compareval = ChgVal(1.0 - gifColor.GetBrightness(),mins[idInd],maxs[idInd]);
                    if (frameInd == 0 || preval != compareval || frameInd == (ids.Count-1))
                    {
                        listval[idInd].Add(FtoTime(frameInd), compareval);
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

        /// <summary>
        /// フレーム数を時間に変換する
        /// </summary>
        /// <param name="flame">何フレーム目か</param>
        /// <returns>1フレームの時間はFpsの逆</returns>
        private double FtoTime(int flame)
        {
            double ans = (1.0 / Fps) * flame;
            return ans;
        }
    }

}
