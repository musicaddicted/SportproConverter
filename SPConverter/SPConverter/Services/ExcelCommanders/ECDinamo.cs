using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;
using SPConverter.Services.Dictionaries;
using System.Collections;

namespace SPConverter.Services.ExcelCommanders
{

    public class ECDinamo : BaseExcelCommander
    {
        internal override int PriceColumn => 10;

        internal override int FirstRow => 11;

        internal override int ArticulColumn => -1;

        internal override int NameColumn => 1;

        private Stack<Category> _categoriesStack = new Stack<Category>();

        public override void Parse()
        {
            Income.Products = new List<Product>();

            int addedCount = 0;
            int skippedCount = 0;
            int usedRangeRows = ActiveWorksheet.UsedRange.Rows.Count;

            for (int i = FirstRow; i < usedRangeRows; i++)
            {
                if (GetCellColor(i, 1) == Color.FromArgb(242, 241, 217))
                {
                    UpdateCategories(GetCellValue(i, 1));
                    continue;
                }

                OnSetProgressBarValue(CalcProgressBarValue(i, usedRangeRows));
                OnPrintStatus($"Обработка позиции {i} из {usedRangeRows}");
                


                string originalName = GetCellValue(i, NameColumn);

                Brand brand = GetBrand(originalName);
                if (brand == null)
                {
                    //skippedCount++;
                    continue;
                }

                string articulValue = GetArticul(originalName, brand);

                string nameValue = GetName(originalName, brand);

                string price = GetCellValue(i, 10);

                var remains = GetRemains(i, GetCategoriesTree().ToUpper().Contains("ОБУВЬ") || (brand.Name == "Asics" && nameValue.Contains("Стелька анатомическая")));

                Product newProduct = new Product
                {
                    Categories = GetCategoriesTree(),
                    Articul = articulValue,
                    Name = nameValue,
                    Brand = brand?.Name,
                    Price = price,
                    //PriceWithSale = priceWithSale,
                    Remains = remains
                };
                Income.Products.Add(newProduct);
                addedCount++;
            }
            OnPrintMessage($"Обработано успешно: {addedCount}; Пропущено: {skippedCount}");

        }

        private void UpdateCategories(string categoryString)
        {
            var newCategory = new Category()
            {
                OriginalName = categoryString,
                CleanName = GetCleanCategory(categoryString)
            };

            while (_categoriesStack.Count != 0)
            {
                if (!newCategory.FirstBlock.StartsWith(_categoriesStack.Peek().FirstBlock))
                {
                    _categoriesStack.Pop();
                }
                else
                {
                    if (!string.IsNullOrEmpty(newCategory.CleanName))
                        // если категория совпала, то нет смысла её добавлять
                        if (newCategory.CleanName != _categoriesStack.Peek().CleanName)
                            _categoriesStack.Push(newCategory);
                    break;
                }
            }

            if (_categoriesStack.Count == 0)
            {
                _categoriesStack.Push(newCategory);
            }
        }

        private string GetCleanCategory(string categoryString)
        {
            // отрежем цифры
            categoryString = categoryString.Substring(categoryString.IndexOf(' ')+1);

            var brand = BrandsDictionary.Instance.Brands.Find(b => categoryString.ToUpper().Contains(b.Name.ToUpper()));

            // 1) не содержит бренда (01 ОБУВЬ СПОРТИВНАЯ) -> возвращаем как есть
            if (brand == null)
                return categoryString;

            // 2) бренд в начале (0512 ASICS НОГА) -> возвращаем всё, что после бренда
            //  Фэйл с 03611 ASICS AW15 и 0366 TORNADO (Россия)				

            if (categoryString.ToUpper().StartsWith(brand.Name.ToUpper()))
                return string.Empty;
            //{
            //    categoryString = categoryString.Substring(brand.Name.Length + 1);
            //    return categoryString;
            //}

            // 3) бренд полностью (0431 MIKASA) -> не нужна категория
            if (categoryString.Trim().ToUpper() == brand.Name.ToUpper())
                return string.Empty;

            // 4) бренд в конце (0120 Обувь волейбольная MIZUNO AW15) -> возвращаем всё, что до бренда
            string[] splits = categoryString.Split(new[] {brand.Name.ToUpper()}, StringSplitOptions.RemoveEmptyEntries);
            return splits[0].Trim();
        }

        private string GetCategoriesTree()
        {
            var clonedStack = new Stack<Category>(_categoriesStack.Reverse());

            string result = "";
            while (clonedStack.Count >0)
            {
                var cat = clonedStack.Pop();
                result = ">" + cat.CleanName + result;
            }
            return result.TrimStart('>');
        }

        public override void Export()
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
                        price = p.Price;
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

                    sw.WriteLine($"{p.Categories};{p.Brand};{p.Articul};{p.Name};{p.FullDescription};{p.ShortDescription};{price};;{p.RemainsTotalCount};{attrib};{PrintPointsWithCommas(4)}");
                    bool firstRow = true;

                    if (variative)
                        foreach (var remain in p.Remains)
                        {
                            string defAttr = firstRow ? p.DefaultAttribute : "";
                            sw.WriteLine(
                                $"{PrintPointsWithCommas(2)}{p.Articul};;;;{p.Price};;{remain.Quantity};{remain.Size.Replace(',', '.')};{PrintPointsWithCommas(3)}{p.Articul};{defAttr}");
                            firstRow = false;
                        }
                }

            }
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

        private List<Remain> GetRemains(int row, bool isShoes)
        {
            var result = new List<Remain>();
            int colFrom;
            int colTill;
            int sizeTypeRow;

            if (isShoes)
            {
                colFrom = 14;
                colTill = 41;
                sizeTypeRow = 0;
            }
            else
            {
                colFrom = 15;
                colTill = 25;
                sizeTypeRow = 1;
            }

            // 1. Если итого по нулям - то по нулям
            var absoluteSum = GetCellValue(row, 42);
            if (string.IsNullOrEmpty(absoluteSum))
            {
                result.Add(new Remain() {Quantity = 0});
                return result;
            }

            // 2. Если указано в 13й колонке - это безразмер
            var sum = GetCellValue(row, 13);
            if (!string.IsNullOrEmpty(sum))
            {
                int quantity = 0;
                bool ok = int.TryParse(sum, out quantity);
                if (!ok)
                    OnPrintMessage(
                        $"Строка {row}, столбец 13, значение = '{sum}': Невозможно преобразовать значение кол-ва в целое число. Будет записано '0'");
                result.Add(new Remain() {Quantity = quantity});
                return result;
            }


            for (int col = colFrom; col <= colTill; col++)
            {
                var exactSizeQuantity = GetCellValue(row, col);
                int quantity = 0;
                if (!string.IsNullOrEmpty(exactSizeQuantity))
                {
                    bool ok = int.TryParse(exactSizeQuantity, out quantity);
                    if (!ok)
                        OnPrintMessage(
                            $"Строка {row}, столбец {col}, значение = '{exactSizeQuantity}': Невозможно преобразовать значение кол-ва в целое число. Будет записано '0'");
                }

                var remain = new Remain { Quantity = quantity };

                string header = GetCellValue(8, col);

                var splits = header.Split(new char[] {'\n'}, StringSplitOptions.None);
                remain.Size = splits[sizeTypeRow];

                result.Add(remain);
            }

            return result;
        }

        private string GetArticul(string originalName, Brand brand)
        {
            string result = string.Empty;
            

            if (brand == null)
                return originalName;

            if (brand.BlocksCountInArticul == 0)
                return originalName;

            // пока что принимаем, что бренд всегда первый
            originalName = originalName.Substring(brand.Name.Length);

            if (brand.BlocksCountInArticul == 0)
                return originalName;

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
                return originalName;

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
            // костыль для русской "с"
            originalName = originalName.Replace("Kv.Rezaс", "Kv.Rezac");

            var brand = BrandsDictionary.Instance.Brands.Find(b => originalName.ToUpper().Contains(b.Name.ToUpper()));

            if (brand == null)
            {
                // TODO
                // предлагать заносить в справочник 

                OnPrintMessage($"Не удалось вычислить бренд из наименования {originalName}");
                return null;
            }


            return brand;
        }

        /// <summary>
        /// Получить склеенную строку категорий.
        /// Для этого перемещаемся к заголовкам, проходим их все
        /// TODO: заменять заглавные на прописные
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="brand"></param>
        /// <returns></returns>
        private string GetConcatCategories(int rowNumber, string brand)
        {
            string result = String.Empty;
            List<string> categories = new List<string>();
            bool stop = false;

            int categoryNextNumbersCount = 100;
            do
            {
                rowNumber --;
                if (GetCellColor(rowNumber, 1) == Color.FromArgb(242, 241, 217))
                {
                    string categoryFullValue = GetCellValue(rowNumber,NameColumn);
                    // возможно следует передавать оригинальное имя бренда с наименования
                    if (categoryFullValue.Contains(brand.ToUpper()))
                    {
                        // отрезаем бренд и всё что за ним
                        string[] splitStrings = categoryFullValue.Split(new string[] {brand}, StringSplitOptions.None);
                        categoryFullValue = splitStrings[0];
                    }

                    // кол-во цифр категории
                    var numbersCount = categoryFullValue.IndexOf(' ');
                    if (numbersCount > categoryNextNumbersCount)
                        continue;

                    categories.Add(categoryFullValue.Substring(numbersCount).Trim());
                    // записываем категорию без номера в начале
                    //result += categoryFullValue.Substring(numbersCount).Trim();
                    categoryNextNumbersCount = numbersCount - 1;

                    if (numbersCount == 2)
                        stop = true;
                    //else
                        //result += ">";
                }

            } while (!stop);

            categories.Reverse();

            categories.ForEach(b =>
            {
                result += b + '>';
            });
            result = result.TrimEnd('>');

            return result;
        }
    }
}
