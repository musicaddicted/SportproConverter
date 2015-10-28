using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using SPConverter.Model;

namespace SPConverter.Services
{
    public abstract class BaseExcelCommander: IDisposable
    {
        public event Action<string> PrintMessage;

        internal Application App;
        internal Workbook Workbook;
        internal Worksheet ActiveWorksheet;
        internal Income Income;

        private int _firstRow = -1;
        private int _articulColumn = -1;
        private int _descriptionColumn = -1;

        #region Cell addresses

        internal virtual int FirstRow
        {
            get
            {
                if (_firstRow == -1)
                    _firstRow = ActiveWorksheet.Cells.Find(What: "Артикул").Row + 1;

                return _firstRow;
            }
        }

        internal virtual int ArticulColumn
        {
            get
            {
                if (_articulColumn == -1)
                    _articulColumn = ActiveWorksheet.Cells.Find(What: "Артикул").Column;

                return _articulColumn;
            }
        }

        internal virtual int NameColumn
        {
            get
            {
                if (_descriptionColumn == -1)
                    _descriptionColumn = ActiveWorksheet.Cells.Find(What: "Номенклатура").Column;

                return _descriptionColumn;
            }
        }

        internal abstract int PriceColumn { get; }

        #endregion

        protected void OnPrintMessage(string message)
        {
            Action<string> action = PrintMessage;
            action?.Invoke(message);
        }


        public void OpenFile(Income income)
        {
            App = new Application {Visible = true};
            Income = income;

            Workbook = App.Workbooks.Open(income.FilePath);
            ActiveWorksheet = Workbook.ActiveSheet;

        }

        public void Dispose()
        {
            App.Quit();
        }

        public virtual void Parse()
        {
            Income.Products = new List<Product>();

            int addedCount = 0;
            int skippedCount = 0;

            for (int i = FirstRow; i < ActiveWorksheet.UsedRange.Rows.Count; i++)
            {
                string articulValue = GetCellValue(i, ArticulColumn);
                string nameValue = GetCellValue(i, NameColumn);
                string priceValue = GetCellValue(i, PriceColumn);

                if (string.IsNullOrEmpty(articulValue) || 
                    string.IsNullOrEmpty(nameValue) ||
                    string.IsNullOrEmpty(priceValue))
                {
                    PrintMessage?.Invoke($"Строка {i} пропущена. Одно из значений в строке нулевое");
                    skippedCount++;
                    continue;
                }

                Income.Products.Add(new Product
                {
                    Articul = articulValue,
                    Name = nameValue,
                    Price = priceValue
                });
                addedCount++;
            }

            Export();

            OnPrintMessage(
                $" * Обработка завершена. * \r\nДобавлено продуктов: {addedCount}\r\nПропущено строк: {skippedCount}\r\n");
        }

        public virtual void Export()
        { }


        //private void Export()
        //{
        //    string exportDir = @"e:\Projects\SportproConverter\Docs\Test\";

        //    string filePath = Path.Combine(exportDir,
        //        String.Format("{0}_processed_{1}.xls", Path.GetFileNameWithoutExtension(Income.FileName),
        //            System.Guid.NewGuid().ToString().Substring(0, 8)));

        //    PrintMessage($"Сохраняем в {filePath}");

        //    try
        //    {
        //        Workbook = App.Workbooks.Add();
        //        ActiveWorksheet = Workbook.ActiveSheet;

        //        for (int i = 1; i < Income.Products.Count - 1; i++)
        //        {
        //            ((Range) ActiveWorksheet.Cells[i, 1]).Value = Income.Products[i - 1].Articul;
        //            ((Range) ActiveWorksheet.Cells[i, 2]).Value = Income.Products[i - 1].Name;
        //            ((Range) ActiveWorksheet.Cells[i, 3]).Value = Income.Products[i - 1].Size;
        //            ((Range) ActiveWorksheet.Cells[i, 4]).Value = Income.Products[i - 1].Price;
        //        }

        //        Workbook.SaveAs(filePath);
        //        PrintMessage("Готово!");
        //    }
        //    catch (Exception exception)
        //    {
        //        PrintMessage(exception.ToString());
        //    }
        //}

        internal string GetCellValue(int row, int column)
        {
            try
            {
                return ((Range)ActiveWorksheet.Cells[row, column]).Value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal Color GetCellColor(int row, int column)
        {
            try
            {
                var colorValue = (int) ((Range) ActiveWorksheet.Cells[row, column]).Interior.Color;
                var excelColor = System.Drawing.ColorTranslator.FromOle(colorValue);
                return excelColor;
            }
            catch (Exception ex)
            {
                return Color.White;
            }
        }

    }
}
