namespace GetParameterCS
{
    partial class FormEditID
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblGifName = new System.Windows.Forms.Label();
            this.DgvEdit = new System.Windows.Forms.DataGridView();
            this.BtnSetDefault = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // lblGifName
            // 
            this.lblGifName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGifName.Location = new System.Drawing.Point(0, 0);
            this.lblGifName.Margin = new System.Windows.Forms.Padding(3);
            this.lblGifName.Name = "lblGifName";
            this.lblGifName.Size = new System.Drawing.Size(800, 28);
            this.lblGifName.TabIndex = 0;
            this.lblGifName.Text = "パラメータ定義";
            this.lblGifName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DgvEdit
            // 
            this.DgvEdit.AllowUserToAddRows = false;
            this.DgvEdit.AllowUserToDeleteRows = false;
            this.DgvEdit.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvEdit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DgvEdit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvEdit.DefaultCellStyle = dataGridViewCellStyle2;
            this.DgvEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvEdit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DgvEdit.GridColor = System.Drawing.Color.Black;
            this.DgvEdit.Location = new System.Drawing.Point(0, 28);
            this.DgvEdit.MinimumSize = new System.Drawing.Size(280, 120);
            this.DgvEdit.Name = "DgvEdit";
            this.DgvEdit.RowHeadersVisible = false;
            this.DgvEdit.RowHeadersWidth = 62;
            this.DgvEdit.RowTemplate.Height = 27;
            this.DgvEdit.Size = new System.Drawing.Size(800, 422);
            this.DgvEdit.TabIndex = 1;
            this.DgvEdit.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvEdit_CellClick);
            // 
            // BtnSetDefault
            // 
            this.BtnSetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSetDefault.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnSetDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnSetDefault.Location = new System.Drawing.Point(571, 0);
            this.BtnSetDefault.Name = "BtnSetDefault";
            this.BtnSetDefault.Size = new System.Drawing.Size(229, 29);
            this.BtnSetDefault.TabIndex = 2;
            this.BtnSetDefault.Text = "この設定をデフォルトにする";
            this.BtnSetDefault.UseVisualStyleBackColor = false;
            this.BtnSetDefault.Click += new System.EventHandler(this.BtnSetDefault_Click);
            // 
            // FormEditID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnSetDefault);
            this.Controls.Add(this.DgvEdit);
            this.Controls.Add(this.lblGifName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 120);
            this.Name = "FormEditID";
            this.Text = "パラメータ設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditID_FormClosing);
            this.Load += new System.EventHandler(this.FormEditID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblGifName;
        private System.Windows.Forms.DataGridView DgvEdit;
        private System.Windows.Forms.Button BtnSetDefault;
    }
}