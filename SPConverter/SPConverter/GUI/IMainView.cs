using System;
using System.Collections.Generic;
using SPConverter.Model;

namespace SPConverter
{
    public interface IMainView
    {
        event Action<Income> ConvertClick;
        event Action StopClick;

        List<IncomeFileType> IncomeFileTypes { set; }

        void PrintLog(string message);
        int ProgressBarValue { set; }
        string StatusMessage { set; }

        bool StopButtonEnabled { get;set; }
        bool ConvertButtonEnabled { get; set; }

    }
}