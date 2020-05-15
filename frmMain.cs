using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// VB版の移植するもの
namespace GetParameterCS
{
    public partial class FrmMain : Form
    {
        #region "変数"
        // フォーム設定
        DataSetting dataSetting;

        // スタイル
        readonly DataGridViewCellStyle compStyle = new DataGridViewCellStyle();
        // マウスクリック位置
        Point MD = new Point();
        Point MU = new Point();
     
        bool view = false;
        Bitmap bmp;
        Image image;
        int showRow;
        readonly string ExePath;

        #endregion

        #region "フォーム処理"
        public FrmMain()
        {
            InitializeComponent();
            ExePath = AppDomain.CurrentDomain.BaseDirectory;
            view = false;
            // スタイルの設定
            compStyle.BackColor = Color.Yellow;

            if (File.Exists(ExePath + "Setting.json"))
            {
                using (StreamReader sr = new StreamReader(ExePath + "Setting.json"))
                {
                    string input = sr.ReadToEnd();
                    dataSetting = JsonConvert.DeserializeObject<DataSetting>(input);

                }
            }
            // フォーム設定ファイルがなければ現在の状態で作成
            else
            {
                dataSetting = new DataSetting()
                {
                    WindowHeight = Height,
                    WindowWidth = Width,
                    WindowLocation = Location,
                    OutputDir = ExePath,
                    IDs = DefaultIDSetting
                };
                // 設定ファイルをインデント、改行をつけて出力
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };

                string output = JsonConvert.SerializeObject(dataSetting,settings);
                output = output.Replace("  ", "\t");
                using (StreamWriter sw = new StreamWriter(ExePath + "Setting.json"))
                {
                    sw.Write(output);
                }
            }
            // セーブダイアログ設定
            saveFileDialog1.InitialDirectory = dataSetting.OutputDir;
            saveFileDialog1.Filter = "モーション設定ファイル(*.motion3.json)|*.motion3.json";
            saveFileDialog1.Title = "保存先のファイルを選択してください";
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataSetting.WindowHeight = Height;
            dataSetting.WindowWidth = Width;
            dataSetting.WindowLocation = Location;

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            string output = JsonConvert.SerializeObject(dataSetting, settings);
            output = output.Replace("  ", "\t");
            using (StreamWriter sw = new StreamWriter(ExePath + "Setting.json"))
            {
                sw.Write(output);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

            // formの形を変えておく
            Height = dataSetting.WindowHeight;
            Width = dataSetting.WindowWidth;
            Location = dataSetting.WindowLocation;

            DataGridViewCell cell = new DataGridViewTextBoxCell();
            var DtColumn = new DataGridViewColumn
            {
                Visible = false,
                Name = "DgvDtSetID",
                CellTemplate = cell
            };
            dgvFiles.Columns.Add(DtColumn);

        }

        #endregion

        #region "ドラッグドロップ"

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
                    lblCaption.Text = "gif/webm/mp4をドラッグ＆ドロップ";
                }
                // 将来用
                else if (Path.GetExtension(fileName) == ".webm" || Path.GetExtension(fileName) == ".mp4")
                {
                    AdddgvRow(fileName);
                    lblCaption.Text = "gif/webm/mp4をドラッグ＆ドロップ";
                }
                else
                {
                    lblCaption.Text = "gif,webm,mp4以外が含まれていました";
                }
            }
        }
        #endregion

        /// <summary>
        /// データグリッドビュー内クリック処理
        /// </summary>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 編集状態を固定する
            dgvFiles.EndEdit();
            // ヘッダー行の場合何もしない
            if (e.RowIndex < 0)
            {
                return;
            }

            var DgvRow = dgvFiles.Rows[e.RowIndex];
            // Fpsがおかしい場合も何もしない
            if (!float.TryParse(DgvRow.Cells["DgvFps"].Value.ToString(), out float Fps))
            {
                MessageBox.Show("Fpsの値が不適切です。数字を入力してください");
                return;
            }



            // 削除　画像も消す。
            if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnDel")
            {
                dgvFiles.CurrentCellChanged -= new EventHandler(DgvFiles_CurrentCellChanged);

                dgvFiles.Rows.RemoveAt(e.RowIndex);
                pictureBox1.Visible = false;
                lblCaption.Visible = true;
                lblCaption.Text = "ファイルをドラッグ＆ドロップ";

                dgvFiles.CurrentCellChanged += new EventHandler(DgvFiles_CurrentCellChanged);

            }
            // セーブ箇所設定（ファイル出力）ボタン
            else if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnSave")
            {

                string filePath = DgvRow.Cells["DgvFilePath"].Value.ToString();
                string rectAngle = DgvRow.Cells["DgvPoints"].Value.ToString();
                if (rectAngle == "") { return; }

                MD = (Point)DgvRow.Cells["StartPoint"].Value;
                MU = (Point)DgvRow.Cells["EndPoint"].Value;
                

                saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(filePath);
                saveFileDialog1.InitialDirectory = dataSetting.OutputDir;
                
                // 動画を読み込み処理
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 選択されたフォルダを記録
                    dataSetting.OutputDir = Path.GetDirectoryName(saveFileDialog1.FileName);

                    Stream stream = saveFileDialog1.OpenFile();
                    if (stream == null) return;
                    DataTable editTable = (DataTable)DgvRow.Cells["DgvDtSetID"].Value;


                    // 輝度の値だけ取得
                    ClsBrightness brightness = new ClsBrightness(filePath, MD, MU);
                    List<List<float>> frames = brightness.Read();
#if DEBUG
                    Writefloatcsv(frames, ExePath + "\\brightframes.csv");
#endif
                    // パラメータを変換
                    ConvBrighttoLive2D conv = new ConvBrighttoLive2D(editTable);
                    frames = conv.Comvert(frames);
#if DEBUG
                    Writefloatcsv(frames, ExePath + "\\live2Dframe.csv");
#endif
                    // 情報をLive2Dモーションクラスに展開
                    ClsLive2DMotion clsLive2D = new ClsLive2DMotion(editTable, Fps);
                    Motion3Json motion3Json = clsLive2D.GetMotions(frames);

                    // シリアライズ出力
                    string json = JsonConvert.SerializeObject(motion3Json, Formatting.Indented);
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(json);
                        Console.WriteLine("出力");
                    }

                    MessageBox.Show("motion3.jsonを出力しました");
                    DgvRow.Cells["DgvStatus"].Value = "変換・保存済み";
                    DgvRow.Cells["DgvStatus"].Style = compStyle;
                }
            }
            // ID名編集ボタン
            else if (dgvFiles.Columns[e.ColumnIndex].Name == "DgvBtnSetID")
            {

                // 子フォームを出す。遷移先でDataTable(?)を書き換え？
                // 行データにDataTableを連結する？
                using (FormEditID editID = new FormEditID())
                {
                    editID.EditTable = (DataTable)DgvRow.Cells["DgvDtSetID"].Value;
                    editID.DataSetting = this.dataSetting;
                    editID.ShowDialog(this);
                    DgvRow.Cells["DgvDtSetID"].Value = editID.EditTable;
                    this.dataSetting = editID.DataSetting;
                }
                // セットされた値で戻ってくる
            }
            else
            {
                ShowSS(DgvRow.Cells["DgvFilePath"].Value.ToString(),e.RowIndex);
            }
        }

        // 別の行をクリックされたらgif画像を切り替える
        private void DgvFiles_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvFiles.CurrentCell != null & dgvFiles.CurrentRow.Index >= 0)
            {
                string fileName = dgvFiles.CurrentRow.Cells["DgvFilePath"].Value.ToString();
                ShowSS(fileName, dgvFiles.CurrentRow.Index);
            }
        }

        private DataTable NewSetID()
        {
            var SetID = new DataTable();
            // SettingsからデフォルトのID名、最大値、最小値を取得してセッティングする
            SetID.Columns.Add("ID", Type.GetType("System.String"));
            SetID.Columns.Add("Min", Type.GetType("System.Int32"));
            SetID.Columns.Add("Max", Type.GetType("System.Int32"));
            SetID.Columns.Add("Tolerance", Type.GetType("System.Double"));

            for (int j = 0;j < dataSetting.IDs.Count; j++)
            {
                var dt = dataSetting.IDs[j];
                if (dt.ID.Length != 0)
                {
                    SetID.Rows.Add(dt.ID, dt.Min, dt.Max,dt.PerTolerance);
                }
                else
                {
                    SetID.Rows.Add("", 0, 0, 0.0);
                }
            }
            return SetID;
        }

        private void AdddgvRow(string fileName)
        {
            //dgvFiles.Rows.Add(Path.GetFileName(fileName), "座標未指定", "保存", "編集" ,"×", fileName, "", NewSetID());

            //var newRow = dgvFiles.Rows[dgvFiles.RowCount - 1];
            dgvFiles.CurrentCellChanged -= new EventHandler(DgvFiles_CurrentCellChanged);

            dgvFiles.Rows.Add();
            var newRow = dgvFiles.Rows[dgvFiles.RowCount - 1];
            newRow.Cells["DgvFileName"].Value = Path.GetFileName(fileName);
            newRow.Cells["DgvStatus"].Value = "専用モデルを選択してください";
            newRow.Cells["DgvFps"].Value = 30.0;
            newRow.Cells["DgvBtnSave"].Value = "保存";
            newRow.Cells["DgvBtnSetID"].Value = "編集";
            newRow.Cells["DgvBtnDel"].Value = "×";
            newRow.Cells["DgvDTSetID"].Value = NewSetID();
            newRow.Cells["DgvFilePath"].Value = fileName;
            newRow.Cells["DgvPoints"].Value = "";
            newRow.Cells["StartPoint"].Value = "";
            newRow.Cells["EndPoint"].Value = "";

            // まだ範囲を設定されていないので保存ボタンをコントロール不可能っぽく

            DataTable table = (DataTable)dgvFiles.CurrentRow.Cells["DgvDtSetID"].Value;
            
            ShowSS(fileName, dgvFiles.Rows.Count - 1);
            dgvFiles.CurrentCellChanged += new EventHandler(DgvFiles_CurrentCellChanged);
        }

        #region "短形選択"

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
            dgvFiles.Rows[showRow].Cells["StartPoint"].Value = MD;
            dgvFiles.Rows[showRow].Cells["EndPoint"].Value = MU;

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

        #endregion

        #region "描画"

        /// <summary>
        /// 一フレーム目のスクリーンショットを表示
        /// </summary>
        /// <param name="mediapath"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool ShowSS(string mediapath, int row = 1)
        {
            try
            {
                pictureBox1.Visible = true;
                lblCaption.Visible = false;

                switch (Path.GetExtension(mediapath))
                {
                    case ".gif":
                        if(image!=null)image.Dispose();
                        if(bmp!=null)bmp.Dispose();
                        image = Image.FromFile(mediapath);
                        bmp = new Bitmap(image.Width, image.Height);
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.DrawImage(image, new Point(0, 0));
                        }
                        break;
                    case ".mp4":
                    case ".webm":
                        using (var capture = new OpenCvSharp.VideoCapture(mediapath))
                        {
                            var img = new OpenCvSharp.Mat();
                            capture.Read(img);

                            if(image!=null)image.Dispose();
                            if(bmp!=null)bmp.Dispose();
                            image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img);
                            bmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img);
                        }
                        break;
                    default:
                        break;
                }



                // 既に短形指定済みなら
                if (dgvFiles.Rows[row].Cells["StartPoint"].Value.ToString() != "")
                {
                    Point start = (Point)dgvFiles.Rows[row].Cells["StartPoint"].Value;
                    Point end = (Point)dgvFiles.Rows[row].Cells["EndPoint"].Value;

                    // 領域を描画
                    DrawRegion(start, end);


                    //PictureBox1に表示する
                    pictureBox1.Image = bmp;
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

        private void GetRegion(Point p1, Point p2, ref Point start, ref Point end)
        {
            start.X = Math.Min(p1.X, p2.X);
            start.Y = Math.Min(p1.Y, p2.Y);

            end.X = Math.Max(p1.X, p2.X);
            end.Y = Math.Max(p1.Y, p2.Y);
        }

        private void DrawRegion(Point start, Point end)
        {
            Pen blackPen = new Pen(Color.Black,1);
            Pen RedPen = new Pen(Color.Red, 3);

            Graphics g = Graphics.FromImage(bmp);

            // 描画する線を点線に設定
            RedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            // 画面を消去
            g.Clear(SystemColors.Control);

            // 領域を描画
            g.DrawImage(image, new Point(0,0));
            g.DrawRectangle(RedPen, start.X, start.Y, Math.Abs(start.X-end.X), Math.Abs(start.Y-end.Y));

#if DEBUG
            DrawPoints(g,start,end);
#endif

            g.Dispose();
        }

        private void DrawPoints(Graphics g,Point start, Point end)
        {
            Pen blackPen = new Pen(Color.Black, 1);
            ClsBrightness brightness = new ClsBrightness("", start, end);
            var points = brightness.Baddr();
            foreach (var point in points)
            {
                g.DrawLine(blackPen, point.X - 5, point.Y - 5, point.X + 5, point.Y + 5);
                g.DrawLine(blackPen, point.X + 5, point.Y - 5, point.X - 5, point.Y + 5);
            }
        }

        #endregion

        #region #デバッグ用"
        private void Writefloatcsv(List<List<float>> data,string path)
        {

            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var frame in data)
                {
                    string line = "";
                    foreach (var param in frame)
                    {
                        if (line != "") line += ",";
                        line += param;
                    }
                    sw.WriteLine(line);
                }
            }
        }
        #endregion

        // 埋め込み Live2d Cubism 3 公式パラメータ準拠
        public List<IDSetting> DefaultIDSetting
        {
            get
            {
                List<IDSetting> iDSettings = new List<IDSetting>
                {
                    new IDSetting("ParamAngleX", -30, 30),
                    new IDSetting("ParamAngleY", -30, 30),
                    new IDSetting("ParamAngleZ", -30, 30, 0.06),
                    new IDSetting("ParamBodyAngleX", -10, 10, 0.06),
                    new IDSetting("ParamBodyAngleZ", -10, 10, 0.06),
                    new IDSetting("ParamBreath", 0, 1),
                    new IDSetting("ParamEyeLOpen", 0, 1),
                    new IDSetting("ParamEyeROpen", 0, 1),
                    new IDSetting("ParamEyeBallX", -1, 1),
                    new IDSetting("ParamEyeBallY", -1, 1),

                    new IDSetting("ParamEyeForm", -1, 1),
                    new IDSetting("ParamBrowLY", -1, 1),
                    new IDSetting("ParamBrowRY", -1, 1),
                    new IDSetting("ParamBrowLForm", -1, 1),
                    new IDSetting("ParamBrowRForm", -1, 1),
                    new IDSetting("ParamBrowLAngle", -1, 1),
                    new IDSetting("ParamBrowRAngle", -1, 1),
                    new IDSetting("ParamMouthOpenY", 0, 1),
                    new IDSetting("ParamMouthForm", -1, 1),
                    new IDSetting("ParamHairFront", -1, 1),

                    new IDSetting("ParamHairSide", -1, 1),
                    new IDSetting("ParamHairBack", -1, 1),
                    new IDSetting("ParamHandL", -1, 1),
                    new IDSetting("ParamHandR", -1, 1),
                    new IDSetting("ParamArmLA", -10, 10),
                    new IDSetting("ParamArmRA", -10, 10),
                    new IDSetting("ParamArmLB", -10, 10),
                    new IDSetting("ParamArmRB", -10, 10),
                    new IDSetting("ParamZ", 0, 1),
                    new IDSetting("ParamX", 0, 1),

                    new IDSetting("ParamC", 0, 1),
                    new IDSetting("ParamV", 0, 1),
                    new IDSetting("ParamSZ", 0, 1),
                    new IDSetting("ParamSX", 0, 1),
                    new IDSetting("ParamSC", 0, 1),
                    new IDSetting("ParamSV", 0, 1),
                    new IDSetting("ParamQ", 0, 1),
                    new IDSetting("ParamW", 0, 1),
                    new IDSetting("ParamE", 0, 1),
                    new IDSetting("ParamR", 0, 1),

                    new IDSetting("ParamT", 0, 1),
                    new IDSetting("ParamY", 0, 1),
                    new IDSetting("ParamTongue", 0, 1),
                    new IDSetting("",0,0),
                    new IDSetting("",0,0),

                    new IDSetting("",0,0),
                    new IDSetting("",0,0),
                    new IDSetting("",0,0),
                    new IDSetting("",0,0),
                    new IDSetting("",0,0)
                };

                return iDSettings;
            }
        }

        private void lblCaption_Click(object sender, EventArgs e)
        {

        }
    }
}
