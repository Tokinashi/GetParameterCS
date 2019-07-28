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
        public long AllValCnt { get; set; }
        private readonly List<Idproperty> Idprop;

        //public ClsReadParameter(Image gif, string rectangle, double dblfps = 30.0)
        //{
        //    Fps = dblfps;
        //    Idprop = Idproperties(rectangle);
        //    SetVals(gif, Idprop);
        ////}
        
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="gif">読み込むgifフルパス</param>
        /// <param name="rectangle">短形選択位置情報</param>
        /// <param name="dblfps">ＦＰＳ</param>
        public ClsReadParameter(Image gif, string rectangle, DataTable editTable, double dblfps = 30.0)
        {
            Fps = dblfps;
            Idprop = Setidproperties(editTable, rectangle);
            SetVals(gif, Idprop);
        }
        public ClsReadParameter(Image gif, Point MD,Point MU, DataTable editTable, double dblfps = 30.0)
        {
            Fps = dblfps;
            Idprop = Setidproperties(editTable, MD, MU);
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
        private List<Idproperty> Setidproperties(DataTable editTable, Point MD, Point MU, int rowCnt = 10)
        {
            var idProp = new List<Idproperty>();

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
        private void SetVals(Image gif, List<Idproperty> lstidproperty, double tolerancePer = 5.0)
        {
            tolerancePer /= 100.0;
            // フレーム数取得
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int frameCnt = gif.GetFrameCount(fd);

            

            Duration = FtoTime(frameCnt - 1);
            AllValCnt = 0;
            // 各フレーム毎に
            for (int frameInd = 0; frameInd < frameCnt; frameInd++)
            {
                gif.SelectActiveFrame(fd, frameInd);
                Bitmap bitmap = (Bitmap)gif;
                // 各ＩＤ順に記録
                for (int idInd = 0; idInd < lstidproperty.Count; idInd++)
                {
                    // 画素取得位置
                    Point idPoint = lstidproperty[idInd].point;
                    // 最大最小
                    double min = lstidproperty[idInd].min;
                    double max = lstidproperty[idInd].max;

                    // ここまでの取得パラメータ推移辞書
                    Dictionary<double, double> timevalue = lstidproperty[idInd].timeVal;

                    // パラメータを取得
                    Color gifColor = bitmap.GetPixel(idPoint.X, idPoint.Y);
                    // Black : 0 White : 1
                    double compareval = ChgVal(gifColor.GetBrightness(), min, max);

                    // 前フレームとの差異
                    bool isTolerance = false;
                    if (frameInd > 0)
                    {
                        if (timevalue.Last().Value != compareval)
                        {
                            double dblMargin = Math.Abs(timevalue.Last().Value - compareval);
                            // 許容誤差範囲をパーセント表記から変更
                            if (dblMargin > (max - min) * tolerancePer)
                            {
                                isTolerance = true;
                                // 直前の点も記録
                                if (timevalue.Last().Key != FtoTime(frameInd - 1))
                                {
                                    timevalue.Add(FtoTime(frameInd - 1), timevalue.Last().Value);
                                }
                            }
                        }
                        
                    }

                    // 最初か最後か値が違ったとき
                    if (isTolerance || frameInd == 0 || frameInd == frameCnt - 1)
                    {
                        timevalue.Add(FtoTime(frameInd), compareval);
                        AllValCnt++;
                    }
                }
            }
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

        /// <summary>
        /// 内部クラス　ＩＤ情報クラス
        /// </summary>
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
