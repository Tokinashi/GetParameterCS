using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

// 動画と短形位置から50種の輝度のリストを作る
namespace GetParameterCS
{
    class ClsBrightness
    {
        private string moviepath;
        private Image image;

        private Point MD;
        private Point MU;

        public List<float> frame;
        public List<List<float>> frames;

        public ClsBrightness(string moviepath, Point mD, Point mU)
        {
            this.moviepath = moviepath;
            MD = mD;
            MU = mU;
            frames = new List<List<float>>();
        }

        public List<List<float>> Read()
        {
            switch (Path.GetExtension(moviepath))
            {
                case ".gif":
                    return Read(Image.FromFile(moviepath));
            // その他webm・mp4なら
                case ".webm": 
                case ".mp4":

                    break;
                // ない場合はエラー
                default:
                    break;
            }
            return null;
        }

        public List<List<float>> Read(Image gif)
        {
            // フレーム数取得
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int frameCnt = gif.GetFrameCount(fd);
            // パラメータを取得する位置を計算しリスト化
            var idaddr = Baddr();

            // 各フレーム毎に
            for (int frameInd = 0; frameInd < frameCnt; frameInd++)
            {
                gif.SelectActiveFrame(fd, frameInd);
                Bitmap bitmap = (Bitmap)gif;
                var monochromes = ReadAllParam(bitmap, idaddr);
                frames.Add(monochromes);
            }
            return frames;
        }

        /// <summary>
        /// bitmapフレーム一枚から位置リストの数だけパラメータを取る
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="points">取得位置リスト</param>
        /// <returns>1フレーム分の輝度情報</returns>
        private List<float> ReadAllParam(Bitmap bitmap,List<Point> points)
        {
            List<float> monochromes = new List<float>();
            // 各ＩＤ順に記録
            for (int i = 0; i < points.Count; i++)
            {
                // パラメータを取得
                Color gifColor = bitmap.GetPixel(points[i].X, points[i].Y);
                // Black : 0 White : 1
                float compareval = gifColor.GetBrightness();

                // 20190815 すべてフレーム番号で撮るリスト
                monochromes.Add(compareval);
            }
            return monochromes;
        }
        
        /// <summary>
        /// 輝度を取得する位置をリスト化
        /// </summary>
        /// <param name="rowCnt"></param>
        /// <returns></returns>
        public List<Point> Baddr(int rowCnt=10)
        {
            List<Point> points = new List<Point>();

            int mHeight = Math.Abs(MD.Y - MU.Y);
            int sq = mHeight / rowCnt;

            for (int j = 0; j < 50; j++)
            {
                int row = 1 + j % rowCnt;
                int col = 1 + j / rowCnt;
                Point point = new Point(MD.X + (col * sq) - (sq / 2), MD.Y + (row * sq) - (sq / 2));
                points.Add(point);
            }

            return points;
        }
    }
}
