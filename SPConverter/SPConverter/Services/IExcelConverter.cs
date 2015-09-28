using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using SPConverter.Model;

namespace SPConverter.Services
{
    public interface IExcelConverter
    {
        void CreateApp();
        void CloseApp();

        void Convert(Income income);
    }
}
