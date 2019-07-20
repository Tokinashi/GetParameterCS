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

        private DataGridViewCellStyle HeaderStyle;

        public FormEditID()
        {
            InitializeComponent();
        }

        private void FormEditID_Load(object sender, EventArgs e)
        {
            HeaderStyle = new DataGridViewCellStyle();
            HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
    }
}
