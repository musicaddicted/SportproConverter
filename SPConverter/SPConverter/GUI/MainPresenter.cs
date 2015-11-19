using System;
using System.Collections.Generic;
using System.ComponentModel;
using SPConverter.Model;
using SPConverter.Services;
using SPConverter.Services.Dictionaries;

namespace SPConverter
{
    public class MainPresenter
    {
        private readonly IMainView _view;

        ExcelConverter _excelConverter;
        private Income _income;

        BackgroundWorker _backgroundWorker;

        public MainPresenter(IMainView view)
        {
            Test();

            _view = view;
            _view.ConvertClick += OnConvertClick;
            _view.StopClick += OnStopClick;
        }

        private void Test()
        {

            var catalog = CatalogDictionary.Instance.Catalog;
            var testIncomeCats = new List<string>
            {
                "обувь",
                "волейбольная",
                "кроссовая"
            };

            DoPrint(catalog.Categories, testIncomeCats);
        }

        private void DoPrint(List<Category> categories, List<string> incomeCategories)
        {

            foreach (Category category in categories)
            {
                category.Tags.ForEach(t=> Console.Write($"{t}({category.MatchCount(incomeCategories)})>"));

                Console.WriteLine();
                DoPrint(category.Categories, incomeCategories);
            }
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
            _view.ConvertButtonEnabled = true;
            _view.StopButtonEnabled = false;

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
            _view.StatusMessage = "";

            
        }

        private void OnConvertClick(Income income)
        {
            _income = income;
            _view.ConvertButtonEnabled = false;
            _view.StopButtonEnabled = true;

            _excelConverter = new ExcelConverter(income.Type);
            _excelConverter.SetProgressBarValue += Converter_SetProgressBarValue;
            _excelConverter.PrintStatus += ExcelCommander_PrintStatus;
            _excelConverter.PrintMessage += _view.PrintLog;
            _excelConverter.OperationCompleted += Converter_OperationCompleted;

            _backgroundWorker = new BackgroundWorker() {WorkerSupportsCancellation = true};
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerAsync();
        }

        private void OnStopClick()
        {
            _backgroundWorker.CancelAsync();
            _view.ConvertButtonEnabled = true;
            _view.StopButtonEnabled = false;
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