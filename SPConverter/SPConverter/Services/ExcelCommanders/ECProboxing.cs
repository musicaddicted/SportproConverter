using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPConverter.Model;

namespace SPConverter.Services.ExcelCommanders
{
    public class ECProboxing : BaseExcelCommander
    {
        internal override int NameColumn => 2;

        internal override int ArticulColumn => 3;

        internal override int PriceColumn => 4;

        public override void Parse()
        {
            Income.Products = new List<Product>();
            CategoryService categoryService = new CategoryService();
            int addedCount = 0;
            int skippedCount = 0;
            int usedRangeRows = ActiveWorksheet.UsedRange.Rows.Count;
            string previousFirstName = "";

            for (int i = FirstRow; i < usedRangeRows; i++)
            {
                OnSetProgressBarValue(CalcProgressBarValue(i, usedRangeRows));
                OnPrintStatus($"Обработка позиции {i} из {usedRangeRows}");

                string originalName = GetCellValue(i, NameColumn);
                string price = GetCellValue(i, 10);
                

                if (string.IsNullOrEmpty(originalName))
                {
                    skippedCount++;
                    continue;
                }
                string firstName = originalName.Substring(0, originalName.IndexOf(' '));

                if (categoryService.LastResult == CategoryService.CategoryChoiсeResult.Undefined
                    || firstName != previousFirstName)
                    categoryService.ParseCategory(firstName);

            }

        }
    }
}
