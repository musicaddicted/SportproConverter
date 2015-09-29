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
        internal override int PriceColumn=> ActiveWorksheet.Cells.Find(What: "РРЦ").Column;
    }
}
