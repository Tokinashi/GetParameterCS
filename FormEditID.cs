using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetParameterCS
{
    public partial class FormEditID : Form
    {
        //プロパティ
        public DataTable EditTable { get; set; }
        public string GifName { get; set; }

        private readonly DataGridViewCellStyle HeaderStyle = new DataGridViewCellStyle();
        private readonly DataGridViewCellStyle ErrorStyle = new DataGridViewCellStyle();

        public FormEditID()
        {
            InitializeComponent();
            HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ErrorStyle.BackColor = Color.OrangeRed;
        }

        private void FormEditID_Load(object sender, EventArgs e)
        {
            lblGifName.Text = GifName;
            DgvEdit.AutoGenerateColumns = false;
            DgvEdit.DataSource = EditTable;
            DgvEdit.AllowUserToAddRows = false;
            DgvEdit.AllowUserToOrderColumns = false;
            DgvEdit.AllowUserToResizeRows = false;
            DgvEdit.AllowUserToDeleteRows = false;
            DgvEdit.AllowDrop = false;

            // 番号列
            DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
            {
                Name = "Number",
                DefaultCellStyle = HeaderStyle,
                MinimumWidth = 37,
                HeaderText = "項番",
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader

            };
            // ID列
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Name = "ID",
                HeaderText = "ID名",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.NotSortable
    };
            // 最小値
            DataGridViewTextBoxColumn minColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Min",
                Name = "Min",
                HeaderText = "最小値",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
};
            // 最大値
            DataGridViewTextBoxColumn maxcolumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Max",
                Name = "Max",
                HeaderText = "最大値",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            DgvEdit.Columns.Add(numColumn);
            DgvEdit.Columns.Add(idColumn);
            DgvEdit.Columns.Add(minColumn);
            DgvEdit.Columns.Add(maxcolumn);

            // 項番数を編集
            for (int i = 0; i < DgvEdit.Rows.Count; i++)
            {
                DgvEdit[0, i].Value = i + 1;
            }

            DgvEdit.CurrentCellChanged += new EventHandler(DgvEdit_CurrentCellChanged);
        }

        private void FormEditID_FormClosing(object sender, FormClosingEventArgs e)
        {
            DgvEdit.EndEdit();
            // ＩＤ名に重複がないかチェック
            string ret = DistinctCheck();
            if (ret != "")
            {
                MessageBox.Show(ret + "が重複しています");
                e.Cancel = true;
            }
        }
        private string DistinctCheck()
        {
            List<string> lstID = new List<string>();
            foreach (DataRow row in EditTable.Rows){
                if (lstID.Contains(row[0].ToString()))
                {
                    return row[0].ToString();
                }
                else
                {
                    lstID.Add(row[0].ToString());
                }
            }
            return "";
        }
        private void DgvEdit_CurrentCellChanged(object sender, EventArgs e)
        {
            List<string> lstID = new List<string>();
            // 重複があれば背景を灰色にする
            foreach (DataGridViewRow row in DgvEdit.Rows)
            {
                string search = row.Cells["ID"].Value.ToString();
                DataGridViewCell idcell = row.Cells["ID"];
                DataGridViewCell maxcell = row.Cells["Max"];
                DataGridViewCell mincell = row.Cells["Min"];

                if (lstID.Contains(search))
                {
                    int num = lstID.IndexOf(search);
                    idcell.Style = ErrorStyle;
                    DgvEdit.Rows[num].Cells["ID"].Style = ErrorStyle;

                    lstID.Add(search);
                }
                else
                {
                    if (idcell.Style == ErrorStyle)
                    {
                        idcell.Style = DgvEdit.DefaultCellStyle;
                    }

                    lstID.Add(search);
                }
                // 最大最小範囲設定
                if ((int)maxcell.Value <= (int)mincell.Value)
                {
                    maxcell.Style = ErrorStyle;
                    mincell.Style = ErrorStyle;
                }
                else if (maxcell.Style == ErrorStyle)
                {
                    maxcell.Style = DgvEdit.DefaultCellStyle;
                }
                else if (mincell.Style == ErrorStyle)
                {
                    mincell.Style = DgvEdit.DefaultCellStyle;
                }
            }
        }

        private void DgvEdit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                DgvEdit.CurrentCellChanged -= new EventHandler(DgvEdit_CurrentCellChanged);
                DgvEdit.EndEdit();
                DgvEdit.CurrentCell = null;
                DgvEdit.ClearSelection();
                DgvEdit.CurrentCellChanged += new EventHandler(DgvEdit_CurrentCellChanged);
            }
        }
    }
}
