using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;

namespace SPConverter.Services
{
    public class ExcelConverter: IExcelConverter
    {
        private ExcelCommander _excelCommander;

        public void CreateApp()
        {
            _excelCommander = new ExcelCommander();
        }

        public void CloseApp()
        {
            _excelCommander.Dispose();
        }

        public void Convert(Income income)
        {
            _excelCommander.OpenFile(income);
        }
    }
}
