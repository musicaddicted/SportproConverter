using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;

namespace SPConverter
{
    public class Global
    {
        private static Global _instance;

        public List<IncomeFileType> IncomeFileTypes { get; }

        public Global()
        {
            IncomeFileTypes = new List<IncomeFileType>
            {
                new IncomeFileType {Description = "Склад03.08.2015.xls", Id = 1},
                new IncomeFileType {Description = "Спортперсона", Id = 2}
            };
        }


        public static Global Instance => _instance ?? (_instance = new Global());
    }
}
