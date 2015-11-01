using System;
using System.ComponentModel;
using SPConverter.Services;

namespace SPConverter
{
    public class MainPresenter
    {
        private readonly IMainView _view;

        public MainPresenter(IMainView view)
        {
            _view = view;
            _view.ConvertClick += OnConvertClick;
        }

        private void OnConvertClick(Model.Income income)
        {


            IExcelConverter converter = new ExcelConverter();
            converter.PrintMessage += _view.PrintLog;
            converter.SetProgressBarValue += Converter_SetProgressBarValue;

            converter.Init(income.Type);
            converter.Convert(income);
            converter.CloseApp();
        }

        private void Converter_SetProgressBarValue(int value)
        {
            _view.ProgressBarValue = value;

        }
    }
}