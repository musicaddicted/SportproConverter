using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;
using SPConverter.Services.Dictionaries;

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
                string originalName = GetCellValue(i, NameColumn);

                Brand brand = GetBrand(originalName);

                string articulValue = GetArticul(originalName, brand);

                string nameValue = GetName(originalName, brand);

                string concatCategories = GetConcatCategories(i, brand.Name);

                string price = GetCellValue(i, 10);
                string priceWithSale = GetCellValue(i, 12);

                string quantity = GetCellValue(i, 42);

                string sizeValue = GetSizes(i, concatCategories.ToUpper().Contains("ОБУВЬ"));

                Product newProduct = new Product
                {
                    Categories = concatCategories,
                    Articul = articulValue,
                    Name = nameValue,
                    Tags = brand?.Name,
                    Price = price,
                    PriceWithSale = priceWithSale,
                    Quantity = quantity,
                    Size = sizeValue
                };
                Income.Products.Add(newProduct);
            }

        }

        private string GetSizes(int row, bool isShoes)
        {
            string result = "";
            for (int i = 14; i < 41; i++)
            {
                var exactSizeQuantity = GetCellValue(row, i);
                if (!string.IsNullOrEmpty(exactSizeQuantity))
                {
                    string header = GetCellValue(8, i);

                    var splits = header.Split(new char[] {'\n'}, StringSplitOptions.None);
                    if (isShoes)
                        result += splits[0] + "|";
                    else
                        result += splits[1] + "|";
                }
            }
            if (result.Length > 0)
                return result.Remove(result.Length-1);

            return result;
        }

        private string GetArticul(string originalName, Brand brand)
        {
            string result = string.Empty;

            if (brand == null)
                return result;

            // пока что принимаем, что бренд всегда первый
            originalName = originalName.Substring(brand.Name.Length);

            var split = originalName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < brand.BlocksCountInArticul; i++)
            {
                result += split[i] + ' ';
            }

            return result.Trim();
        }

        private string GetName(string originalName, Brand brand)
        {
            string result = string.Empty;

            if (brand == null)
                return result;

            // пока что принимаем, что бренд всегда первый
            originalName = originalName.Substring(brand.Name.Length);

            var split = originalName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = brand.BlocksCountInArticul; i < split.Length; i++)
            {
                result += split[i] + ' ';
            }

            return result.Trim();
        }

        private Brand GetBrand(string originalName)
        {
            var brand = BrandsDictionary.Instance.Brands.Find(b => originalName.ToUpper().Contains(b.Name.ToUpper()));

            //string[] splitStrings = originalName.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            //if (splitStrings.Length == 0)

            if (brand == null)
            {
                // TODO
                // предлагать заносить в справочник 

                //throw new Exception($"Не удалось вычислить бренд из наименования {originalName}");
                OnPrintMessage($"Не удалось вычислить бренд из наименования {originalName}");
                return null;
            }


            return brand;
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
                    result += categoryFullValue.Substring(categoryFullValue.IndexOf(' ')).Trim();
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
