using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;
using SPConverter.Services.ExcelCommanders;

namespace SPConverter.Services
{
    public class ExcelConverter: IExcelConverter
    {
        protected BaseExcelCommander ExcelCommander;

        public ExcelConverter(IncomeFileType incomeFileType)
        {
            Init(incomeFileType);
        }


        public void Init(IncomeFileType incomeFileType)
        {
            ExcelCommanderFactory factory = new ExcelCommanderFactory();
            ExcelCommander = factory.CreateExcelCommander(incomeFileType);
            ExcelCommander.PrintMessage += OnExcelCommander_PrintMessage;
            ExcelCommander.PrintStatus += ExcelCommander_PrintStatus;
            ExcelCommander.SetProgressBarValue += ExcelCommander_SetProgressBarValue;
        }

        private void ExcelCommander_SetProgressBarValue(int value)
        {
            SetProgressBarValue?.Invoke(value);
        }

        private void ExcelCommander_PrintStatus(string statusMessage)
        {
            PrintStatus?.Invoke(statusMessage);
        }

        private void OnExcelCommander_PrintMessage(string message)
        {
            PrintMessage?.Invoke(message);
        }

        public void CloseApp()
        {
            ExcelCommander.Dispose();
        }

        public void Convert(Income income)
        {
            try
            {
                PrintMessage?.Invoke("Открываем файл");
                ExcelCommander.OpenFile(income);
                PrintMessage?.Invoke("Обработка...");
                ExcelCommander.Parse();

                PrintMessage?.Invoke("Выгружаем");
                ExcelCommander.Export();
                PrintMessage?.Invoke("Готово!");
                OperationCompleted?.Invoke(new RunWorkerCompletedEventArgs(null, null, false));
            }
            catch (Exception exception)
            {
                OperationCompleted?.Invoke(new RunWorkerCompletedEventArgs(null, exception, true));
            }
            
        }

        public event Action<string> PrintMessage;
        public event Action<string> PrintStatus;
        public event Action<int> SetProgressBarValue;
        public event Action<RunWorkerCompletedEventArgs> OperationCompleted;
    }
}
