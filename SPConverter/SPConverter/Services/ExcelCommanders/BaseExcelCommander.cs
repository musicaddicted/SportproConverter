using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using SPConverter.Model;

namespace SPConverter.Services
{
    public abstract class BaseExcelCommander: IDisposable
    {
        public event Action<string> PrintMessage;
        public event Action<int> SetProgressBarValue;
        public event Action<string> PrintStatus;

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
            PrintMessage?.Invoke(message);
        }

        protected void OnSetProgressBarValue(int value)
        {
            SetProgressBarValue?.Invoke(value);
        }

        protected void OnPrintStatus(string statusMessage)
        {
            PrintStatus?.Invoke(statusMessage);
        }


        public void OpenFile(Income income)
        {
            App = new Application {Visible = false};
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

        protected int CalcProgressBarValue(int row, int totalCount)
        {
            int res = 0;
            return Math.DivRem(row * 100, totalCount, out res);
        }

        public virtual void Export()
        {
            string exportFilePath = Path.Combine(Global.Instance.RootDir,
               $"{Path.GetFileNameWithoutExtension(Income.FileName)}_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.csv");

            using (StreamWriter sw = new StreamWriter(exportFilePath, false, Encoding.GetEncoding(1251)))
            {

                //sw.WriteLine("Категория;Артикул;Метки товара;Наименование;Подробное описание;Краткое описание;Цена;Цена со скидкой;Кол-во на складе;Цвет;Размер;Перекрестные товары;Картинка;ATTACHMENT;ATTACHMENT;Статус товара;SEOTITLE;SEODESC;SEOKW");
                //                      1   2       3           4               5               6           7       8                   9           10            11            12      13              14          15
                sw.WriteLine("Категория;Бренды;Артикул;Наименование;Подробное описание;Краткое описание;Цена;Цена со скидкой;Кол-во на складе;Attribs;Перекрестные товары;Картинка;Статус товара;sku_parent;default_attr");
                foreach (Product p in Income.Products)
                {
                    string allSizesString = "";
                    string attrib = "";
                    string price = "";
                    bool variative = true;

                    if (p.Remains.Count == 1 && string.IsNullOrEmpty(p.Remains[0].Size))
                    {
                        attrib = "";
                        //price = p.Price;
                        variative = false;
                    }
                    else
                    {
                        p.Remains.ForEach(r =>
                        {
                            allSizesString += r.Size.Replace(',', '.') + ":";
                        });
                        allSizesString = allSizesString.TrimEnd(':');
                        attrib = $"*Размер:{allSizesString}";
                    }

                    sw.WriteLine($"{p.Categories};{p.Brand};{p.Articul};{p.Name};{p.FullDescription};{p.ShortDescription};{p.Price};;{p.RemainsTotalCount};{attrib};{PrintPointsWithCommas(4)}");
                    bool firstRow = true;

                    if (!variative) continue;
                    foreach (var remain in p.Remains)
                    {
                        string defAttr = firstRow ? p.DefaultAttribute : "";
                        sw.WriteLine(
                            $"{PrintPointsWithCommas(2)}{p.Articul};;;;{p.Price};;{remain.Quantity};{remain.Size.Replace(',', '.')};{PrintPointsWithCommas(3)}{p.Articul};{defAttr}");
                        firstRow = false;
                    }
                }

            }
            OnPrintMessage($"Файл успешно выгружен в {exportFilePath}");
        }

        private string PrintPointsWithCommas(int count)
        {
            string res = "";
            for (int i = 0; i < count; i++)
            {
                res += ";";
            }
            return res;
        }


        internal string GetCellValue(int row, int column)
        {
            try
            {
                var range = (Range) ActiveWorksheet.Cells[row, column];
                return range.Value2 == null ? string.Empty : range.Value2.ToString();
            }
            catch (Exception ex)
            {
                OnPrintMessage($"В методе GetCellValue row = {row}, column = {column}. {ex}");
                return null;
            }
        }

        /// <summary>
        /// Получить уровень дерева для строки
        /// </summary>
        internal double GetOutlineLevel(int row)
        {
            try
            {
                var range = (Range)ActiveWorksheet.Cells[row, 1];
                return range.Rows.OutlineLevel;
            }
            catch (Exception ex)
            {
                OnPrintMessage($"В методе GetOutlineLevel row = {row}. {ex}");
                return -1;
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
                OnPrintMessage($"В методе GetCellColor row = {row}, column = {column}. {ex}");
                return Color.White;
            }
        }

    }
}
