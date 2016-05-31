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

        private readonly Stack<DinamoCategory> _categoriesStack = new Stack<DinamoCategory>();

        public override void Parse()
        {
            // test category fill
            Income.Products = new List<Product>();
            var
                onlyLeaves =
                    CatalogDictionary.Instance.AllCategoriesList.Where(cat => cat.Categories.Count == 0).ToList();
            onlyLeaves.ForEach(cat =>
            {
                Income.Products.Add(new Product
                {
                    Articul = "test_" + System.Guid.NewGuid().ToString().Substring(0, 8),
                    Brand = "",
                    Categories = cat.PluginExportString,
                    Name = "test name",
                    Price = "0",
                    Remains = new List<Remain>()
                });
            });
            //return;

            Income.Products = new List<Product>();
            CategoryService categoryService = new CategoryService();
            int addedCount = 0;
            int skippedCount = 0;
            bool skipCategoryPrinted = false;
            int usedRangeRows = ActiveWorksheet.UsedRange.Rows.Count;

            for (int i = FirstRow; i < usedRangeRows; i++)
            {
                if (GetCellColor(i, 1) == Color.FromArgb(242, 241, 217))
                {
                    categoryService.LastResult = CategoryService.CategoryChoiсeResult.Undefined;
                    UpdateCategories(GetCellValue(i, 1));
                    skipCategoryPrinted = false;
                    continue;
                }

                OnSetProgressBarValue(CalcProgressBarValue(i, usedRangeRows));
                OnPrintStatus($"Обработка позиции {i} из {usedRangeRows}");

                string originalName = GetCellValue(i, NameColumn);
                string price = GetCellValue(i, 10);

                if (string.IsNullOrEmpty(originalName))
                {
                    skippedCount ++;
                    continue;
                }

                if (categoryService.LastResult == CategoryService.CategoryChoiсeResult.Undefined)
                    categoryService.ParseCategory(_categoriesStack.ToList(), "");

                if (categoryService.LastResult == CategoryService.CategoryChoiсeResult.Ignore)
                {
                    skippedCount++;
                    if (skipCategoryPrinted == false)
                    {
                        OnPrintMessage($"Категорию '{GetCategoriesTree()}' пропускаем");
                        skipCategoryPrinted = true;
                    }
                    continue;
                }

                Brand brand = GetBrand(originalName);
                if (brand == null)
                {
                    skippedCount++;
                    continue;
                }

                string articulValue = GetArticul(originalName, brand);

                string nameValue = GetName(originalName, brand);

                

                var remains = GetRemains(i,
                    GetCategoriesTree().ToUpper().Contains("ОБУВЬ") ||
                    (brand.Name == "Asics" && nameValue.Contains("Стелька анатомическая")), price);

                Product newProduct = new Product
                {
                    Categories = categoryService.ChosenCategoryString,
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
            if (string.IsNullOrEmpty(categoryString))
                return;

            var newCategory = new DinamoCategory()
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
                return UpFirstLetter(categoryString);

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
            return UpFirstLetter(splits[0].Trim());
        }

        private string UpFirstLetter(string inputString)
        {
            string result = inputString[0].ToString().ToUpper();
            result += inputString.Substring(1);
            return result;
        }


        private string GetCategoriesTree()
        {
            var clonedStack = new Stack<DinamoCategory>(_categoriesStack.Reverse());

            string result = "";
            while (clonedStack.Count >0)
            {
                var cat = clonedStack.Pop();
                result = ">" + cat.CleanName + result;
            }
            return result.TrimStart('>');
        }



        private List<Remain> GetRemains(int row, bool isShoes, string price)
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
                result.Add(new Remain() {Quantity = 0, Price = price});
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
                result.Add(new Remain() {Quantity = quantity, Price = price});
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

                var remain = new Remain { Quantity = quantity, Price = price};

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
    }
}
