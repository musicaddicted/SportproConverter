using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;

namespace SPConverter.Services
{
    class Tester
    {
        public static void Test()
        {
            Income testIncome = new Income()
            {
                FilePath = @"e:\Projects\SportproConverter\Docs\Test\Склад03.08.2015.xls",
                Type = Global.Instance.IncomeFileTypes.Find(t => t.Id == 1)
            };

            IExcelConverter converter = new ExcelConverter();

            converter.CreateApp();
            converter.Convert(testIncome);
            converter.CloseApp();
        }

    }
}
