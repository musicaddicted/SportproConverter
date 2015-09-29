using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;
using SPConverter.Services.ExcelCommanders;

namespace SPConverter.Services
{
    public class ExcelConverter: IExcelConverter
    {
        private BaseExcelCommander _excelCommander;

        public void Init(IncomeFileType incomeFileType)
        {
            ExcelCommanderFactory factory = new ExcelCommanderFactory();
            _excelCommander = factory.CreateExcelCommander(incomeFileType);
        }

        public void CloseApp()
        {
            _excelCommander.Dispose();
        }

        public void Convert(Income income)
        {
            _excelCommander.OpenFile(income);
            _excelCommander.Parse(income);
        }
    }
}
