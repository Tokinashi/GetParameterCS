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

        private void DebugMethod()
        {
            string csvPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Default.csv";
            string gifPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Facerig.gif";
            MakePara(gifPath, csvPath, 30);

        }

        public FrmMain()
        {
            InitializeComponent();
            view = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            DebugMethod();
        }

        private void MakePara(string gif,string csv,double fps = 30.0)
        {

            ClsReadParameter clsRead = new ClsReadParameter(gif, csv, fps);
            ClsWriteJson writeJson = new ClsWriteJson(clsRead.Duration, fps, clsRead.AllValCnt);
            List<string> Ids = clsRead.ids;
            for (int i = 0; i < clsRead.GetCount(); i++)
            {
                Dictionary<double, double> aVal= clsRead.dicTimeVal[i];
                writeJson.AddPoint(Ids[i], aVal.Keys.ToList(), aVal.Values.ToList());
            }
            writeJson.WriteJson(Path.GetFileNameWithoutExtension(gif));
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
            if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnDel")
            {
                dgvFiles.Rows.RemoveAt(e.RowIndex);
            }
            else if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnSave")
            {

            }
            else
            {
                ShowGif(dgvFiles.Rows[e.RowIndex].Cells["DgvFilePath"].Value.ToString(),e.RowIndex);
            

            }
        }
        private void AdddgvRow(string fileName)
        {
            dgvFiles.Rows.Add(Path.GetFileName(fileName),"未変換","保存","×", fileName, "");
            dgvFiles.CurrentCell = dgvFiles.Rows[dgvFiles.Rows.Count - 1].Cells[0];
            ShowGif(fileName,dgvFiles.Rows.Count-1);

        }

        private bool ShowGif(string gif,int row = 1)
        {
            try
            {
                image = Image.FromFile(gif);
                bmp = new Bitmap(image.Width, image.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(image, new Point(0, 0));
                    g.Dispose();
                }
                pictureBox1.Image = bmp;
                showRow = row;
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
