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
                ConvertClick(new Income
                {
                    FilePath = @"e:\Projects\SportproConverter\Docs\Test\Остатки ДИНАМО.xlsx",
                    Type = Global.Instance.IncomeFileTypes.Find(t => t.Description == "Динамо")
                });
        }

        private void AddMessage(string message)
        {
            rtbLog.Text += message + Environment.NewLine;
        }

        private void SetProgressBarValue(int value)
        {
            progressBar.Value = value;
        }

        private void SetStatus(string status)
        {
            labelStatus.Text = status;
        }


        public event Action<Income> ConvertClick;

        public void PrintLog(string message)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(AddMessage), message);
            else
                AddMessage(message);
        }

        public int ProgressBarValue
        {
            get { return progressBar.Value; }
            set
            {
                if (InvokeRequired)
                    Invoke(new Action<int>(SetProgressBarValue), value);
                else
                    SetProgressBarValue(value);
            }
        }

        public string StatusMessage
        {
            get { return labelStatus.Text; }
            set
            {
                if (InvokeRequired)
                    Invoke(new Action<string>(SetStatus), value);
                else
                    SetStatus(value);
            }
        }

        private void btOpenIncomeFile_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
