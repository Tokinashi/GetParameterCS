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
        public DataSetting DataSetting { get; set; }
        private DataTable FirstTB { get; set; }

        private readonly DataGridViewCellStyle HeaderStyle = new DataGridViewCellStyle();
        private readonly DataGridViewCellStyle NumStyle = new DataGridViewCellStyle();
        private readonly DataGridViewCellStyle ErrorStyle = new DataGridViewCellStyle();
        public FormEditID()
        {
            InitializeComponent();
            HeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            NumStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

            FirstTB = EditTable.Copy();

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
                DefaultCellStyle = NumStyle,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                SortMode = DataGridViewColumnSortMode.NotSortable
};
            // 最大値
            DataGridViewTextBoxColumn maxcolumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Max",
                Name = "Max",
                HeaderText = "最大値",
                DefaultCellStyle = NumStyle,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            DataGridViewTextBoxColumn pertolcolumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Tolerance",
                Name = "Tol",
                HeaderText = "許容誤差（1.00=100%）",
                DefaultCellStyle = NumStyle,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            DgvEdit.Columns.Add(numColumn);
            DgvEdit.Columns.Add(idColumn);
            DgvEdit.Columns.Add(minColumn);
            DgvEdit.Columns.Add(maxcolumn);
            DgvEdit.Columns.Add(pertolcolumn);

            // 項番数を編集
            for (int i = 0; i < DgvEdit.Rows.Count; i++)
            {
                DgvEdit[0, i].Value = i + 1;
            }

            DgvEdit.CurrentCellChanged += new EventHandler(DgvEdit_CurrentCellChanged);
        }

        private void FormEditID_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ = DgvEdit.EndEdit();
            CheckError();
            // ＩＤ名に重複がないかチェック
            bool ret = DistinctCheck();
            if (!ret)
            {
                DialogResult result = MessageBox.Show("不正な設定があります\r\n最初の設定に戻しますか？", 
                    "設定エラー", 
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ResetDT(FirstTB);
                    DgvEdit.CurrentCell = DgvEdit[0, 0];
                }
                
                e.Cancel = true;
            }
        }

        private void ResetDT(DataTable dt)
        {
            for (int i = 0; i < DgvEdit.RowCount; i++)
            {
                DataGridViewRow row = DgvEdit.Rows[i];
                DataRow dtrow = dt.Rows[i];

                //TODO:パラメータ移し替え処理

                row.Cells["ID"].Value = dtrow["ID"];
                row.Cells["Max"].Value = dtrow["Max"];
                row.Cells["Min"].Value = dtrow["Min"];
                row.Cells["Tol"].Value = dtrow["Tolerance"];

                row.Cells["ID"].Style = DgvEdit.DefaultCellStyle;
                row.Cells["Max"].Style = NumStyle;
                row.Cells["Min"].Style = NumStyle;
                row.Cells["Tol"].Style = NumStyle;

            }
            CheckError();
        }

        private bool DistinctCheck()
        {
            foreach (DataGridViewRow row in DgvEdit.Rows)
            {
                if(row.Cells["ID"].Style == ErrorStyle)
                {
                    Console.WriteLine(row.Index + ":ID:");
                    return false;
                }
                if(row.Cells["Max"].Style == ErrorStyle)
                {
                    Console.WriteLine(row.Index + ":Max");
                    return false;
                }
                if(row.Cells["Min"].Style == ErrorStyle)
                {
                    Console.WriteLine(row.Index + ":Min:");
                    return false;
                }
                if(row.Cells["Tol"].Style == ErrorStyle)
                {
                    Console.WriteLine(row.Index + ":Tol:");
                    return false;
                }
            }
            return true;
        }
        private void DgvEdit_CurrentCellChanged(object sender, EventArgs e)
        {
            CheckError();
        }

        private void CheckError()
        {

            List<string> lstID = new List<string>();
            // 重複があればセルの色を変える
            foreach (DataGridViewRow row in DgvEdit.Rows)
            {
                string search = row.Cells["ID"].Value.ToString();
                DataGridViewCell idcell = row.Cells["ID"];
                DataGridViewCell maxcell = row.Cells["Max"];
                DataGridViewCell mincell = row.Cells["Min"];
                DataGridViewCell tolcell = row.Cells["Tol"];
                if (search == "")
                {
                    continue;
                }
                else if (lstID.Contains(search) && search != "")
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
                    maxcell.Style = NumStyle;
                    mincell.Style = NumStyle;
                }

                // 許容誤差が0～1から外れている
                if (0 > (double)tolcell.Value || (double)tolcell.Value > 1.0)
                {
                    tolcell.Style = ErrorStyle;
                }
                else if (tolcell.Style == ErrorStyle)
                {
                    tolcell.Style = NumStyle;
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

        private void BtnSetDefault_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < DataSetting.IDs.Count; j++)
            {
                DataRow dtrow = EditTable.Rows[j];
                var dt = DataSetting.IDs[j];

                dt.ID = (string)dtrow["ID"];
                dt.Min = (int)dtrow["Min"];
                dt.Max = (int)dtrow["Max"];
                dt.PerTolerance = (double)dtrow["Tolerance"];
            }
        }
    }
}
