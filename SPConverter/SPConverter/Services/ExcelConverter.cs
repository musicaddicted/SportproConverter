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
            _excelCommander.PrintMessage += OnExcelCommander_PrintMessage;
            _excelCommander.SetProgressBarValue += OnExcelCommander_SetProgressBarValue;
        }

        private void OnExcelCommander_SetProgressBarValue(int value)
        {
            SetProgressBarValue?.Invoke(value);
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
            SetProgressBarValue?.Invoke(0);
            PrintMessage?.Invoke("Export");
            _excelCommander.Export();
            PrintMessage?.Invoke("Done!");
        }

        public event Action<string> PrintMessage;
        public event Action<int> SetProgressBarValue;
    }
}
