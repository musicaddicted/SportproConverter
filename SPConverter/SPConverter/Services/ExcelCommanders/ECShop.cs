using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SPConverter.Model;
using SPConverter.Services.Dictionaries;

namespace SPConverter.Services.ExcelCommanders
{
    public class ECShop : BaseExcelCommander
    {
        internal override int PriceColumn => 9;

        internal override int ArticulColumn => -1;

        internal override int FirstRow => 8;

        internal override int NameColumn => 1;

        internal int QuantityColumn => 6;

        private readonly Stack<DinamoCategory> _categoriesStack = new Stack<DinamoCategory>();

        public override void Parse()
        {
            Income.Products = new List<Product>();
            CategoryService categoryService = new CategoryService();
            int addedCount = 0;
            int skippedCount = 0;
            bool skipCategoryPrinted = false;
            int usedRangeRows = ActiveWorksheet.UsedRange.Rows.Count;

            for (int i = FirstRow; i < usedRangeRows; i++)
            {
                string quantityString = GetCellValue(i, QuantityColumn);

                if (string.IsNullOrEmpty(quantityString))
                {
                    categoryService.LastResult = CategoryService.CategoryChoiсeResult.Undefined;
                    UpdateCategories(GetCellValue(i, 1), GetOutlineLevel(i));
                    skipCategoryPrinted = false;
                    continue;
                }

                string price = GetCellValue(i, PriceColumn);

                OnSetProgressBarValue(CalcProgressBarValue(i, usedRangeRows));
                OnPrintStatus($"Обработка позиции {i} из {usedRangeRows}");

                string originalName = _categoriesStack.Pop().CleanName;
                string articul = GetCellValue(i, NameColumn);
                

                if (string.IsNullOrEmpty(originalName))
                {
                    skippedCount++;
                    continue;
                }

                if (categoryService.LastResult == CategoryService.CategoryChoiсeResult.Undefined)
                    categoryService.ParseCategory(_categoriesStack.ToList(), originalName);

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

                var remains = new List<Remain>();

                if (!string.IsNullOrEmpty(quantityString))
                {
                    int quantity = 0;
                    int.TryParse(quantityString, out quantity);
                    if (quantity < 0)
                        quantity = 0;
                    remains.Add(new Remain() {Quantity = quantity});
                }


                Product newProduct = new Product
                {
                    Categories = categoryService.ChosenCategoryString,
                    Articul = articul,
                    Name = originalName,
                    Brand = brand?.Name,
                    Price = price,
                    Remains = remains
                };
                Income.Products.Add(newProduct);
                addedCount++;
            }
            OnPrintMessage($"Обработано успешно: {addedCount}; Пропущено: {skippedCount}");
        }
        

        private Brand GetBrand(string originalName)
        {
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

        private void UpdateCategories(string categoryString, double level)
        {
            if (string.IsNullOrEmpty(categoryString))
                return;

            var newCategory = new DinamoCategory()
            {
                OriginalName = categoryString,
                CleanName = GetCleanCategory(categoryString)
            };

            int offset = 1;

            if (_categoriesStack.Count == 0)
            {
                _categoriesStack.Push(newCategory);
            }
            else
            {
                while (_categoriesStack.Count >= level - offset)
                {
                    _categoriesStack.Pop();
                }
                if (!string.IsNullOrEmpty(newCategory.CleanName))
                    // если категория совпала, то нет смысла её добавлять
                    if (newCategory.CleanName != _categoriesStack.Peek().CleanName)
                        _categoriesStack.Push(newCategory);
            }
        }

        private string GetCleanCategory(string categoryString)
        {
            return categoryString;

            //// отрежем цифры
            //categoryString = categoryString.Substring(categoryString.IndexOf(' ') + 1);

            //var brand = BrandsDictionary.Instance.Brands.Find(b => categoryString.ToUpper().Contains(b.Name.ToUpper()));

            //// 1) не содержит бренда (01 ОБУВЬ СПОРТИВНАЯ) -> возвращаем как есть
            //if (brand == null)
            //    return UpFirstLetter(categoryString);

            //// 2) бренд в начале (0512 ASICS НОГА) -> возвращаем всё, что после бренда
            ////  Фэйл с 03611 ASICS AW15 и 0366 TORNADO (Россия)				

            //if (categoryString.ToUpper().StartsWith(brand.Name.ToUpper()))
            //    return string.Empty;
            ////{
            ////    categoryString = categoryString.Substring(brand.Name.Length + 1);
            ////    return categoryString;
            ////}

            //// 3) бренд полностью (0431 MIKASA) -> не нужна категория
            //if (categoryString.Trim().ToUpper() == brand.Name.ToUpper())
            //    return string.Empty;

            //// 4) бренд в конце (0120 Обувь волейбольная MIZUNO AW15) -> возвращаем всё, что до бренда
            //string[] splits = categoryString.Split(new[] { brand.Name.ToUpper() }, StringSplitOptions.RemoveEmptyEntries);
            //return UpFirstLetter(splits[0].Trim());
        }

        private string GetCategoriesTree()
        {
            var clonedStack = new Stack<DinamoCategory>(_categoriesStack.Reverse());

            string result = "";
            while (clonedStack.Count > 0)
            {
                var cat = clonedStack.Pop();
                result = ">" + cat.CleanName + result;
            }
            return result.TrimStart('>');
        }
    }
}