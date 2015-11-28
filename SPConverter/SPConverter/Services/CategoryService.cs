using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;
using SPConverter.Services.Dictionaries;

namespace SPConverter.Services
{
    public class CategoryService
    {
        public string ParseCategory(List<DinamoCategory> dinamoCategories)
        {
            CatalogDictionary.Instance.AllCategoriesList.ForEach(
                dictCat => dictCat.SetMatchCount(dinamoCategories.ConvertAll(c => c.CleanName)));

            var maxMatchCount = CatalogDictionary.Instance.AllCategoriesList.Max(c => c.MatchCount);



            // если совсем ничего не совпало, предлагаем пользователю выбрать вручную
            if (maxMatchCount == 0)
            {

                return "";
            }
            var bestList = CatalogDictionary.Instance.AllCategoriesList.Where(c => c.MatchCount == maxMatchCount).ToList();

            if (bestList.Count > 1)
            {
                // у нас несколько победителей. Пускай пользователь решает что с ними делать

            }
            return bestList[0].PluginExportString;

            //var newList = AllCategoriesList.OrderByDescending(c => c.MatchCount(categoriesList)).ToList();

        }

    }
}
