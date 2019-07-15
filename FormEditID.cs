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
        public DataTable editTable { get; set; }

        public FormEditID()
        {
            InitializeComponent();
        }

        private void FormEditID_Load(object sender, EventArgs e)
        {
            DgvEdit.AutoGenerateColumns = false;
            DgvEdit.DataSource = editTable;

            // 番号列
            DataGridViewTextBoxColumn numColumn = new DataGridViewTextBoxColumn
            {
                Name = "Number",
                HeaderText = "項番",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            
            // ID列
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ID",
                Name = "ID",
                HeaderText = "ID名",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            // 最小値
            DataGridViewTextBoxColumn minColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Min",
                Name = "Min",
                HeaderText = "最小値",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            // 最大値
            DataGridViewTextBoxColumn maxcolumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Max",
                Name = "Max",
                HeaderText = "最大値",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
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
    }
}
