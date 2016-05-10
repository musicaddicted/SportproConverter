using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;

namespace SPConverter.Services.ExcelCommanders
{
    internal class ExcelCommanderFactory
    {
        public BaseExcelCommander CreateExcelCommander(IncomeFileType incomeFileType)
        {
            switch (incomeFileType.Id)
            {
                case 1:
                    return new ECDinamo();

                case 2:
                    return new ECProboxing();

                case 3:
                    return new ECShop();

                    throw new NotImplementedException(
                        $"Метод разбора файлов типа {incomeFileType.Description} ещё не реализован");
            }
            return null;
        }
    }
}
