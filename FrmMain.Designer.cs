namespace GetParameterCS
{
    partial class FrmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.DgvFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvFps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvBtnSave = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DgvBtnSetID = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DgvBtnDel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DgvDTSetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.dgvFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.splitContainer1.Panel2.Controls.Add(this.lblCaption);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(985, 572);
            this.splitContainer1.SplitterDistance = 80;
            this.splitContainer1.TabIndex = 3;
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvFiles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DgvFileName,
            this.DgvStatus,
            this.DgvFps,
            this.DgvBtnSave,
            this.DgvBtnSetID,
            this.DgvBtnDel,
            this.DgvDTSetID,
            this.DgvFilePath,
            this.DgvPoints,
            this.StartPoint,
            this.EndPoint});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFiles.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvFiles.Location = new System.Drawing.Point(0, 0);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.RowHeadersWidth = 62;
            this.dgvFiles.RowTemplate.Height = 27;
            this.dgvFiles.Size = new System.Drawing.Size(985, 80);
            this.dgvFiles.TabIndex = 0;
            this.dgvFiles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellContentClick);
            this.dgvFiles.CurrentCellChanged += new System.EventHandler(this.DgvFiles_CurrentCellChanged);
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaption.Font = new System.Drawing.Font("游ゴシック Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(985, 488);
            this.lblCaption.TabIndex = 1;
            this.lblCaption.Text = "gifファイルをドラッグ＆ドロップ";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // DgvFileName
            // 
            this.DgvFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DgvFileName.HeaderText = "ファイル";
            this.DgvFileName.MinimumWidth = 83;
            this.DgvFileName.Name = "DgvFileName";
            this.DgvFileName.ReadOnly = true;
            this.DgvFileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DgvStatus
            // 
            this.DgvStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DgvStatus.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgvStatus.HeaderText = "ステータス";
            this.DgvStatus.MinimumWidth = 83;
            this.DgvStatus.Name = "DgvStatus";
            this.DgvStatus.ReadOnly = true;
            this.DgvStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DgvStatus.Width = 83;
            // 
            // DgvFps
            // 
            this.DgvFps.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgvFps.HeaderText = "FPS";
            this.DgvFps.MinimumWidth = 8;
            this.DgvFps.Name = "DgvFps";
            this.DgvFps.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DgvFps.Width = 46;
            // 
            // DgvBtnSave
            // 
            this.DgvBtnSave.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvBtnSave.DefaultCellStyle = dataGridViewCellStyle3;
            this.DgvBtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DgvBtnSave.HeaderText = "保存";
            this.DgvBtnSave.MinimumWidth = 40;
            this.DgvBtnSave.Name = "DgvBtnSave";
            this.DgvBtnSave.ReadOnly = true;
            this.DgvBtnSave.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvBtnSave.Text = "保存";
            this.DgvBtnSave.UseColumnTextForButtonValue = true;
            this.DgvBtnSave.Width = 50;
            // 
            // DgvBtnSetID
            // 
            this.DgvBtnSetID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgvBtnSetID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DgvBtnSetID.HeaderText = "ID編集";
            this.DgvBtnSetID.MinimumWidth = 8;
            this.DgvBtnSetID.Name = "DgvBtnSetID";
            this.DgvBtnSetID.ReadOnly = true;
            this.DgvBtnSetID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvBtnSetID.Text = "編集";
            this.DgvBtnSetID.ToolTipText = "ZXCV,Shift+ZXCVに当てているカスタムパラメータ名を編集します";
            this.DgvBtnSetID.UseColumnTextForButtonValue = true;
            this.DgvBtnSetID.Width = 66;
            // 
            // DgvBtnDel
            // 
            this.DgvBtnDel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgvBtnDel.HeaderText = "";
            this.DgvBtnDel.MinimumWidth = 28;
            this.DgvBtnDel.Name = "DgvBtnDel";
            this.DgvBtnDel.ReadOnly = true;
            this.DgvBtnDel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvBtnDel.Text = "×";
            this.DgvBtnDel.UseColumnTextForButtonValue = true;
            this.DgvBtnDel.Width = 28;
            // 
            // DgvDTSetID
            // 
            this.DgvDTSetID.HeaderText = "DtSetID";
            this.DgvDTSetID.MinimumWidth = 8;
            this.DgvDTSetID.Name = "DgvDTSetID";
            this.DgvDTSetID.Visible = false;
            this.DgvDTSetID.Width = 150;
            // 
            // DgvFilePath
            // 
            this.DgvFilePath.HeaderText = "ファイルパス";
            this.DgvFilePath.MinimumWidth = 8;
            this.DgvFilePath.Name = "DgvFilePath";
            this.DgvFilePath.Visible = false;
            this.DgvFilePath.Width = 150;
            // 
            // DgvPoints
            // 
            this.DgvPoints.HeaderText = "短形選択";
            this.DgvPoints.MinimumWidth = 8;
            this.DgvPoints.Name = "DgvPoints";
            this.DgvPoints.Visible = false;
            this.DgvPoints.Width = 150;
            // 
            // StartPoint
            // 
            this.StartPoint.HeaderText = "StartPoint";
            this.StartPoint.MinimumWidth = 8;
            this.StartPoint.Name = "StartPoint";
            this.StartPoint.Visible = false;
            this.StartPoint.Width = 150;
            // 
            // EndPoint
            // 
            this.EndPoint.HeaderText = "EndPoint";
            this.EndPoint.MinimumWidth = 8;
            this.EndPoint.Name = "EndPoint";
            this.EndPoint.Visible = false;
            this.EndPoint.Width = 150;
            // 
            // FrmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(985, 572);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmMain";
            this.Text = "Moli    -Motion log interface-";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn DgvFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DgvStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DgvFps;
        private System.Windows.Forms.DataGridViewButtonColumn DgvBtnSave;
        private System.Windows.Forms.DataGridViewButtonColumn DgvBtnSetID;
        private System.Windows.Forms.DataGridViewButtonColumn DgvBtnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn DgvDTSetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DgvFilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn DgvPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndPoint;
    }
}

