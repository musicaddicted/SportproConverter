using System;
using SPConverter.Model;

namespace SPConverter
{
    public interface IMainView
    {
        event Action<Income> ConvertClick;

        void PrintLog(string message);
        int ProgressBarValue { set; }
        string StatusMessage { set; }

    }
}