using System;
using System.ComponentModel;
using SPConverter.Model;
using SPConverter.Services;

namespace SPConverter
{
    public class MainPresenter
    {
        private readonly IMainView _view;

        ExcelConverter _excelConverter;
        private Income _income;

        public MainPresenter(IMainView view)
        {
            _view = view;
            _view.ConvertClick += OnConvertClick;
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _excelConverter.Init(_income.Type);
            _excelConverter.Convert(_income);
            _excelConverter.CloseApp();
        }

        private void Converter_OperationCompleted(RunWorkerCompletedEventArgs e)
        {
            _view.ProgressBarValue = 0;
            if (e.Error != null)
            {
                _view.PrintLog($"Произошла ошибка {e.Error}");
                return;
            }

            if (e.Cancelled)
            {
                _view.PrintLog("Операция прервана!");
                return;
            }

            
            _view.PrintLog("Операция завершена!");
        }

        private void OnConvertClick(Income income)
        {
            _income = income;

            _excelConverter = new ExcelConverter(income.Type);
            _excelConverter.SetProgressBarValue += Converter_SetProgressBarValue;
            _excelConverter.PrintStatus += ExcelCommander_PrintStatus;
            _excelConverter.PrintMessage += _view.PrintLog;
            _excelConverter.OperationCompleted += Converter_OperationCompleted;

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += _backgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync();
        }

        private void ExcelCommander_PrintStatus(string statusMessage)
        {
            _view.StatusMessage = statusMessage;
        }

        private void Converter_SetProgressBarValue(int value)
        {
            _view.ProgressBarValue = value;
        }
    }
}