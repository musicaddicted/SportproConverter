using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;
using SPConverter.Services;

namespace SPConverter
{
    public partial class Main : Form, IMainView
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ConvertClick != null)
                ConvertClick(new Income()
                {
                    FilePath = @"e:\Projects\SportproConverter\Docs\Test\Склад03.08.2015.xls",
                    Type = Global.Instance.IncomeFileTypes.Find(t => t.Id == 1)
                });
        }

        private void AddMessage(string message)
        {
            rtbLog.Text += message + Environment.NewLine;
        }


        public event Action<Income> ConvertClick;

        public void PrintLog(string message)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(AddMessage), message);
            else
                AddMessage(message);
        }

        public int ProgressBarValue { get; set; }

        private void btOpenIncomeFile_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
