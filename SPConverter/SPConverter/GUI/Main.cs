using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.GUI;
using SPConverter.Model;
using SPConverter.Services;

namespace SPConverter
{
    public partial class Main : Form, IMainView
    {
        public Main()
        {
            InitializeComponent();
            StopButtonEnabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbIncomeFile.Text.Length == 0)
            {
                MessageBox.Show("Выберите файл остатков","Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(tbIncomeFile.Text))
            {
                MessageBox.Show("Выбранный файл не найден","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            ConvertClick?.Invoke(new Income
            {
                FilePath = tbIncomeFile.Text,
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
        public event Action StopClick;

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

        public bool StopButtonEnabled
        {
            get
            {
                return btStop.Enabled;
            }
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => { btStop.Enabled = value; }));
                else
                    btStop.Enabled = value;
            }
        }

        public bool ConvertButtonEnabled
        {
            get { return btConvert.Enabled; }
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => { btConvert.Enabled = value; }));
                else
                    btConvert.Enabled = value;
            }
        }

        private void btOpenIncomeFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = @"Файлы остатков (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbIncomeFile.Text = ofd.FileName;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            StopClick?.Invoke();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }
    }
}
