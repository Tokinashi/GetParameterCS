using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// VB版の移植するもの
namespace GetParameterCS
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void DebugMethod()
        {
            string csvPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Default.csv";
            string gifPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Facerig.gif";
            ClsReadParameter clsRead = new ClsReadParameter(gifPath, csvPath);
            Console.WriteLine(clsRead.Duration);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DebugMethod();
        }
    }
}
