using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;

namespace SPConverter.Services.ExcelCommanders
{
    public class ECDinamo : BaseExcelCommander
    {
        internal override int PriceColumn => 10;

        internal override int FirstRow => 14;

        internal override int ArticulColumn => -1;

        internal override int NameColumn => 1;

        public override void Parse()
        {
            Income.Products = new List<Product>();

            int addedCount = 0;
            int skippedCount = 0;

            for (int i = FirstRow; i < ActiveWorksheet.UsedRange.Rows.Count; i++)
            {
                string articulValue = ArticulColumn > 0 ? "" : GetCellValue(i, ArticulColumn);

                string originalName = GetCellValue(i, NameColumn);

                string brand = TryGetBrand(originalName);

                string concatCategories = GetConcatCategories(i, brand);



                Product newProduct = new Product()
                {
                    Categories = concatCategories,
                    Articul = articulValue
                };
                Income.Products.Add(newProduct);
            }

        }

        private string TryGetBrand(string originalName)
        {
            string[] splitStrings = originalName.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (splitStrings.Length == 0)
                throw new Exception($"Не удалось вычислить бренд из наименования {originalName}");
            return splitStrings[0];
        }

        /// <summary>
        /// Получить склеенную строку категорий.
        /// Для этого перемещаемся к заголовкам, проходим их все
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="brand"></param>
        /// <returns></returns>
        private string GetConcatCategories(int rowNumber, string brand)
        {
            string result = String.Empty;
            bool stop = false;

            do
            {
                rowNumber --;
                if (GetCellColor(rowNumber, 1) == Color.FromArgb(242, 241, 217))
                {
                    string categoryFullValue = GetCellValue(rowNumber,NameColumn);
                    if (categoryFullValue.Contains(brand))
                    {
                        // отрезаем бренд и всё что за ним
                        string[] splitStrings = categoryFullValue.Split(new string[] {brand}, StringSplitOptions.None);
                        categoryFullValue = splitStrings[0];
                    }

                    // записываем категорию без номера в начале
                    result += categoryFullValue.Substring(categoryFullValue.IndexOf(' '));
                    if (categoryFullValue.IndexOf(' ') == 2)
                        stop = true;
                    else
                        result += ",";
                }

            } while (!stop);

            return result;
        }
    }
}
