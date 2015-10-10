using System;
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

            converter.Init(income.Type);
            converter.Convert(income);
            converter.CloseApp();
        }
    }
}