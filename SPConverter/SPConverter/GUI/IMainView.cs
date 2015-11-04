using System;
using SPConverter.Model;

namespace SPConverter
{
    public interface IMainView
    {
        event Action<Income> ConvertClick;
        event Action StopClick;

        void PrintLog(string message);
        int ProgressBarValue { set; }
        string StatusMessage { set; }

        bool StopButtonEnabled { get;set; }
        bool ConvertButtonEnabled { get; set; }

    }
}