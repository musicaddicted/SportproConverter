using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using SPConverter.Model;

namespace SPConverter.Services
{
    public interface IExcelConverter
    {
        void CloseApp();

        void Convert(Income income);

        event Action<string> PrintMessage;
        event Action<string> PrintStatus;
        event Action<int> SetProgressBarValue;
        event Action<RunWorkerCompletedEventArgs> OperationCompleted;
    }
}
