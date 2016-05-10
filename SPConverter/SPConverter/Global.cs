using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;

namespace SPConverter
{
    public class Global
    {
        private static Global _instance;

        public List<IncomeFileType> IncomeFileTypes { get; }

        public IncomeFileType CurrentType { get; set; }

        public Global()
        {
            IncomeFileTypes = new List<IncomeFileType>
            {
                new IncomeFileType {Id = 1, Description = "Динамо", ChoicesDictionary = "choices.csv"},
                new IncomeFileType {Id = 2, Description = "Proboxing"},
                new IncomeFileType {Id = 3, Description = "Магазин", ChoicesDictionary = "choices.csv"}
            };
        }

        public string RootDir => Application.StartupPath;

        public static Global Instance => _instance ?? (_instance = new Global());
    }
}
