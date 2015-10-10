using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPConverter.Services.ExcelCommanders
{
    /// <summary>
    /// Склад03.08.2015.xls
    /// </summary>
    public class ECWarehouse : BaseExcelCommander
    {
        private int _priceColumn = -1;


        internal override int PriceColumn
        {
            get
            {
                if (_priceColumn == -1)
                    _priceColumn = ActiveWorksheet.Cells.Find(What: "РРЦ").Column;
                return _priceColumn;
            }
        }
    }
}
