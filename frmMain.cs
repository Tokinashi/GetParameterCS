using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

// VB版の移植するもの
namespace GetParameterCS
{
    public partial class FrmMain : Form
    {
        Point MD = new Point();
        Point MU = new Point();
        Bitmap bmp;
        Image image;
        int showRow;
        bool view = false;
        string ExePath;

        //private void DebugMethod()
        //{
        //    ExePath = AppDomain.CurrentDomain.BaseDirectory + "\\";
        //    string csvPath = ExePath + "Default.csv";
        //    string gifPath = ExePath + "Facerig.gif";
        //    MakePara(gifPath, csvPath, 30);

        //}

        public FrmMain()
        {
            InitializeComponent();
            view = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExePath = AppDomain.CurrentDomain.BaseDirectory + "\\";
            saveFileDialog1.InitialDirectory = ExePath;
            saveFileDialog1.Filter = "モーション設定ファイル(*.motion3.json)|*.motion3.json";
            saveFileDialog1.Title = "保存先のファイルを選択してください";
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            //DebugMethod();
        }

        private void Pointtest(string rectangle,int rowCnt = 10)
        {

            string[] rect = rectangle.Split(',');
            Point MD = new Point(int.Parse(rect[0]), int.Parse(rect[1]));
            Point MU = new Point(int.Parse(rect[2]), int.Parse(rect[3]));
            int mHeight = Math.Abs(MD.Y - MU.Y);
            int sq = mHeight / rowCnt;

            int i = 0;
            int row = 1;
            int col = 1;
            for (int j = 1; j <= Properties.Settings.Default.Properties.Count; j++)
            {
                string value = Properties.Settings.Default["ID_" + j].ToString();
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

                using (Graphics g = Graphics.FromImage(image))
                {
                    //Penオブジェクトの作成(幅3黒色)
                    Pen p = new Pen(Color.Red, 1);
                    //(10, 20)-(100, 200)に線を引く
                    g.DrawLine(p, point.X - 5, point.Y - 5, point.X + 5, point.Y + 5);
                    g.DrawLine(p, point.X + 5, point.Y - 5, point.X - 5, point.Y + 5);

                    //リソースを解放する
                    p.Dispose();
                    g.Dispose();
                }
                //PictureBox1に表示する
                pictureBox1.Image = image;

                i++;
                row++;
            }
            Application.DoEvents();
            //foreach (System.Configuration.SettingsProperty item in Properties.Settings.Default.Properties)
            //{


            //    string valueName = item.Name;
            //    string value = Properties.Settings.Default[valueName].ToString();
            //    string[] values = value.Split(',');
            //    string id = values[0];
            //    int min = int.Parse(values[1]);
            //    int max = int.Parse(values[2]);
            //    if (row == 11)
            //    {
            //        row = 1;
            //        col++;
            //    }
            //    Point point = new Point(MD.X + (col * sq) - (sq / 2), MD.Y + (row * sq) - (sq / 2));

            //    Graphics g = Graphics.FromImage(image);
            //    //Penオブジェクトの作成(幅3黒色)
            //    Pen p = new Pen(Color.Black, 3);
            //    //(10, 20)-(100, 200)に線を引く
            //    g.DrawLine(p, 10, 20, 100, 200);

            //    //リソースを解放する
            //    p.Dispose();
            //    g.Dispose();
            //    //PictureBox1に表示する
            //    pictureBox1.Image = image;

            //    i++;
            //    row++;
            //}
        }


        private void MakePara(string gif, string rectangle, double fps = 30.0)
        {
            //Pointtest(rectangle);


            ClsReadParameter clsRead = new ClsReadParameter(Image.FromFile(gif), rectangle, fps);
            ClsWriteJson writeJson = new ClsWriteJson(clsRead.Duration, fps, clsRead.AllValCnt);
            for (int i = 0; i < clsRead.Count(); i++)
            {
                Dictionary<double, double> aVal= clsRead.DicTimeVal(i);
                writeJson.AddPoint(clsRead.Ids(i), aVal.Keys.ToList(), aVal.Values.ToList());
            }
            writeJson.WriteJson(saveFileDialog1.FileName);
            Console.WriteLine("出力");
            //lblStatus.Text = "完了";
        }
        

        private void FrmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FrmMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i];
                if (Path.GetExtension(fileName) == ".gif")
                {
                    AdddgvRow(fileName);
                }
                else
                {
                    lblCaption.Text += "gif以外が含まれていました";
                }
            }
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 削除　基本カレントセルは上に。
            if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnDel")
            {
                dgvFiles.Rows.RemoveAt(e.RowIndex);
                // 一番上だった場合
                if (e.RowIndex == 0)
                {   
                    /// もうデータがない場合
                    if (dgvFiles.Rows.Count == 0)
                    {
                        pictureBox1.Visible = false;
                        lblCaption.Text = "ファイルをドラッグ＆ドロップ";
                    }
                    /// 下のデータが上に繰り上がるのでそちらをカレントセルにする
                    else { dgvFiles.CurrentCell = dgvFiles.Rows[0].Cells[0]; }

                }
                else
                {
                dgvFiles.CurrentCell = dgvFiles.Rows[e.RowIndex-1].Cells[0];

                }
            }
            else if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnSave")
            {
                string filePath = dgvFiles.Rows[e.RowIndex].Cells["DgvFilePath"].Value.ToString();
                string rectAngle = dgvFiles.Rows[e.RowIndex].Cells["DgvPoints"].Value.ToString();
                if (rectAngle == "")
                {
                    return;
                }
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    MakePara(filePath, rectAngle);
                    dgvFiles.Rows[showRow].Cells["DgvStatus"].Value = "変換・保存済み";
                    
                }
            }
            else
            {
                ShowGif(dgvFiles.Rows[e.RowIndex].Cells["DgvFilePath"].Value.ToString(),e.RowIndex);
            

            }
        }
        private void AdddgvRow(string fileName)
        {
            dgvFiles.Rows.Add(Path.GetFileName(fileName), "未変換", "保存", "×", fileName, "");
            dgvFiles.CurrentCell = dgvFiles.Rows[dgvFiles.Rows.Count - 1].Cells[0];
            ShowGif(fileName, dgvFiles.Rows.Count - 1);

        }

        private bool ShowGif(string gif, int row = 1)
        {
            try
            {
                if (pictureBox1.Visible == false) { pictureBox1.Visible = true; }
                image = Image.FromFile(gif);
                bmp = new Bitmap(image.Width, image.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(image, new Point(0, 0));
                    g.Dispose();
                }
                pictureBox1.Image = bmp;
                showRow = row;
                lblCaption.Text = dgvFiles.Rows[row].Cells["DgvFileName"].Value.ToString();
                
                //bmp = new Bitmap(image.Width, image.Height);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
            return true;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 描画フラグON
            view = true;

            // Mouseを押した座標を記録
            MD.X = e.X;
            MD.Y = e.Y;

        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Point start = new Point();
            Point end = new Point();

            // Mouseを離した座標を記録
            MU.X = e.X;
            MU.Y = e.Y;

            //System.Diagnostics.Debug.WriteLine("MouseUp({0},{1})->({2},{3})", MD.X, MD.Y, MU.X, MU.Y);

            // 座標から(X,Y)座標を計算
            GetRegion(MD, MU, ref start, ref end);

            // 領域を描画
            DrawRegion(start, end);

            //PictureBox1に表示する
            pictureBox1.Image = bmp;

            // 描画フラグOFF
            view = false;

            //dgvFilesに記録
            string strPoints = "(" + MD.X.ToString() + "," + MD.Y.ToString() + ") / (" + MU.X.ToString() + "," + MU.Y.ToString() + ")";
            dgvFiles.Rows[showRow].Cells["DgvStatus"].Value = strPoints;
            dgvFiles.Rows[showRow].Cells["DgvPoints"].Value = strPoints.Replace("(","").Replace(")","").Replace("/",",");
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            Point start = new Point();
            Point end = new Point();

            // 描画フラグcheck
            if (view == false)
            {
                return;
            }

            // カーソルが示している場所の座標を取得
            p.X = e.X;
            p.Y = e.Y;

            // 座標から(X,Y)座標を計算
            GetRegion(MD, p, ref start, ref end);

            // 領域を描画
            DrawRegion(start, end);

            //PictureBox1に表示する
            pictureBox1.Image = bmp;
        }
        private void GetRegion(Point p1, Point p2, ref Point start, ref Point end)
        {
            start.X = Math.Min(p1.X, p2.X);
            start.Y = Math.Min(p1.Y, p2.Y);

            end.X = Math.Max(p1.X, p2.X);
            end.Y = Math.Max(p1.Y, p2.Y);
        }
        private int GetLength(int start, int end)
        {
            return Math.Abs(start - end);
        }

        private void DrawRegion(Point start, Point end)
        {
            Pen blackPen = new Pen(Color.Black);
            
            Graphics g = Graphics.FromImage(bmp);

            // 描画する線を点線に設定
            blackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            // 画面を消去
            g.Clear(SystemColors.Control);

            // 領域を描画
            g.DrawImage(image, new Point(0,0));
            g.DrawRectangle(blackPen, start.X, start.Y, GetLength(start.X, end.X), GetLength(start.Y, end.Y));

            g.Dispose();
        }
    }
}
