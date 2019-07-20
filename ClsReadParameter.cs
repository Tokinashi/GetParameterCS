using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.IO;
using System.Linq;

namespace GetParameterCS
{
    /// <summary>
    /// パラメータを読み取り格納するクラス
    /// </summary>
    class ClsReadParameter
    {
        public double Fps { get; }
        public double Duration { get; set; }
        public long AllValCnt { get => allValCnt; set => allValCnt = value; }
        private readonly List<Idproperty> Idprop;
        private long allValCnt;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="gif">読み込むgifフルパス</param>
        /// <param name="rectangle">短形選択位置情報</param>
        /// <param name="dblfps">ＦＰＳ</param>
        public ClsReadParameter(Image gif, string rectangle, double dblfps = 30.0)
        {
            Fps = dblfps;
            Idprop = Idproperties(rectangle);
            SetVals(gif, Idprop);
        }
        public ClsReadParameter(Image gif, string rectangle, DataTable editTable, double dblfps = 30.0)
        {
            Fps = dblfps;
            Idprop = Setidproperties(editTable,rectangle);
            SetVals(gif, Idprop);
        }
        /// <summary>
        /// プロパティ向け、ＩＤが全何種類あるか
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Idprop.Count;
        }
        /// <summary>
        /// ID別で時間とパラ値のセットを取得
        /// </summary>
        /// <param name="i">ID番号</param>
        /// <returns></returns>
        public Dictionary<double, double> DicTimeVal(int i)
        {
            return Idprop[i].timeVal;
        }
        /// <summary>
        /// ID名
        /// </summary>
        /// <param name="i">ID番号</param>
        /// <returns></returns>
        public string Ids(int i)
        {
            return Idprop[i].id;
        }

        private List<Idproperty> Setidproperties(DataTable editTable, string rectangle, int rowCnt = 10)
        {
            var idProp = new List<Idproperty>();

            string[] rect = rectangle.Split(',');
            Point MD = new Point(int.Parse(rect[0]), int.Parse(rect[1]));
            Point MU = new Point(int.Parse(rect[2]), int.Parse(rect[3]));
            int mHeight = Math.Abs(MD.Y - MU.Y);
            int sq = mHeight / rowCnt;
            int i = 0;
            int row = 1;
            int col = 1;

            for (int j = 0; j < editTable.Rows.Count; j++)
            {

                // ID名があれば
                if (editTable.Rows[j][0].ToString().Length > 0)
                {
                    string id = editTable.Rows[j][0].ToString();
                    int min = (int)editTable.Rows[j][1];
                    int max = (int)editTable.Rows[j][2];
                    if (row == 11)
                    {
                        row = 1;
                        col++;
                    }
                    Point point = new Point(MD.X + (col * sq) - (sq / 2), MD.Y + (row * sq) - (sq / 2));
                    
                    idProp.Add(new Idproperty(id, point, min, max, new Dictionary<double, double>()));
                    i++;
                    row++;
                }
            }
            return idProp;
        }


        /// <summary>
        /// gifから各ＩＤ分の時間・パラメータ値の推移を記録
        /// </summary>
        /// <param name="gif">読み取るgif</param>
        /// <param name="lstidproperty">各ＩＤ分の情報</param>
        private void SetVals(Image gif, List<Idproperty> lstidproperty)
        {
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int frameCnt = gif.GetFrameCount(fd);
            Duration = FtoTime(frameCnt - 1);
            AllValCnt = 0;
            for (int frameInd = 0; frameInd < frameCnt; frameInd++)
            {
                gif.SelectActiveFrame(fd, frameInd);
                Bitmap bitmap = (Bitmap)gif;
                for (int idInd = 0; idInd < lstidproperty.Count; idInd++)
                {
                    Point idPoint = lstidproperty[idInd].point;

                    Color gifColor = bitmap.GetPixel(idPoint.X, idPoint.Y);
                    Dictionary<double, double> tv = lstidproperty[idInd].timeVal;
                    double min = lstidproperty[idInd].min;
                    double max = lstidproperty[idInd].max;
                    // 輝度のままに変えました。Black : 0 White : 1
                    double compareval = ChgVal(gifColor.GetBrightness(), min, max);
                    // 最初か最後か値が違ったとき
                    if (frameInd == 0 || tv.Last().Value != compareval || frameInd == frameCnt - 1)
                    {
                        tv.Add(FtoTime(frameInd), compareval);
                        AllValCnt++;
                    }
                }
            }
        }

        /// <summary>
        /// 各ＩＤのＩＤ名、最小値、最大値、取得画素の位置を設定
        /// </summary>
        /// <param name="rectangle">短形選択位置情報</param>
        /// <param name="rowCnt">行カウント設定（デフォルト1列で10行）</param>
        /// <returns></returns>
        private List<Idproperty> Idproperties(string rectangle, int rowCnt = 10)
        {
            List<Point> points = new List<Point>();

            string[] rect = rectangle.Split(',');
            Point MD = new Point(int.Parse(rect[0]), int.Parse(rect[1]));
            Point MU = new Point(int.Parse(rect[2]), int.Parse(rect[3]));
            int mHeight = Math.Abs(MD.Y - MU.Y);
            int sq = mHeight / rowCnt;

            var idProp = new List<Idproperty>();
            int i = 0;
            int row = 1;
            int col = 1;

            for (int j = 1; j <= Properties.Settings.Default.Properties.Count; j++)
            {
                string value = Properties.Settings.Default["ID_" + j].ToString();

                // valueの値がなくなったら終了
                if (value.Length != 0)
                {
                    string[] values = value.Split(',');
                    string id = values[0];
                    int min = int.Parse(values[1]);
                    int max = int.Parse(values[2]);
                    if (row == 11)
                    {
                        row = 1;
                        col++;
                    }
                    Point point = new Point(MD.X + (col * sq) - (sq / 2), MD.Y + (row * sq) - (sq / 2));
                    points.Add(point);
                    idProp.Add(new Idproperty(id, point, min, max, new Dictionary<double, double>()));
                    i++;
                    row++;
                }
                else
                {
                    break;
                }
            }

            return idProp;
        }
        private double ChgVal(double value, double min, double max)
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
        private class Idproperty
        {
            public string id;
            public Point point;
            public double min;
            public double max;
            public Dictionary<double, double> timeVal;

            public Idproperty(string id, Point point, double min, double max, Dictionary<double, double> timeVal)
            {
                this.id = id;
                this.point = point;
                this.min = min;
                this.max = max;
                this.timeVal = timeVal;
            }
        }
    }

}
