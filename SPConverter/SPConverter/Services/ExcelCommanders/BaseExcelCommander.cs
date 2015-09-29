using System;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using SPConverter.Model;

namespace SPConverter.Services
{
    public abstract class BaseExcelCommander: IDisposable
    {
        internal Application App;
        internal Workbook Workbook;
        internal Worksheet ActiveWorksheet;

        #region Cell addresses

        internal virtual int FirstRow => ActiveWorksheet.Cells.Find(What: "Артикул").Row + 1;

        internal virtual int ArticulColumn => ActiveWorksheet.Cells.Find(What: "Артикул").Column;
        internal virtual int DescriptionColumn => ActiveWorksheet.Cells.Find(What: "Номенклатура").Column;
        internal abstract int PriceColumn { get; }

        #endregion

        public void OpenFile(Income income)
        {
            App = new Application {Visible = true};

            Workbook = App.Workbooks.Open(income.FilePath);
            ActiveWorksheet = Workbook.ActiveSheet;

        }

        #region private Wrappers
        private void SetProperty(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            obj.GetType().InvokeMember(sProperty, BindingFlags.SetProperty, null, obj, oParam);
        }
        private object GetProperty(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.GetProperty, null, obj, oParam);
        }
        private object GetProperty(object obj, string sProperty, object oValue1, object oValue2)
        {
            object[] oParam = new object[2];
            oParam[0] = oValue1;
            oParam[1] = oValue2;
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.GetProperty, null, obj, oParam);
        }
        private object GetProperty(object obj, string sProperty)
        {
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.GetProperty, null, obj, null);
        }
        private object InvokeMethod(object obj, string sProperty, object[] oParam)
        {
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }
        private object InvokeMethod(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }
        #endregion

        public void Dispose()
        {
            App.Quit();
        }

        public void Parse(Income income)
        {

            Product firstProduct = new Product
            {
                Articul = GetCellValue(FirstRow, ArticulColumn),
                Description = GetCellValue(FirstRow, DescriptionColumn),
                Price = GetCellValue(FirstRow, PriceColumn)
            };
        }

        internal string GetCellValue(int row, int column)
        {
            return ((Range) ActiveWorksheet.Cells[row, column]).Value.ToString();
        }


    }
}
