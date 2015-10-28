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
        public event EventHandler<string> AddMessage;

        private BaseExcelCommander _excelCommander;

        public void Init(IncomeFileType incomeFileType)
        {
            ExcelCommanderFactory factory = new ExcelCommanderFactory();
            _excelCommander = factory.CreateExcelCommander(incomeFileType);
            _excelCommander.PrintMessage += OnExcelCommander_PrintMessage;

        }

        private void OnExcelCommander_PrintMessage(string message)
        {
            PrintMessage?.Invoke(message);
        }

        public void CloseApp()
        {
            _excelCommander.Dispose();
        }

        public void Convert(Income income)
        {
            PrintMessage?.Invoke("Opening file");
            _excelCommander.OpenFile(income);
            PrintMessage?.Invoke("Parsing file");
            _excelCommander.Parse();
            PrintMessage?.Invoke("Export");
            _excelCommander.Export();
            PrintMessage?.Invoke("Done!");
        }

        public event Action<string> PrintMessage;
    }
}
