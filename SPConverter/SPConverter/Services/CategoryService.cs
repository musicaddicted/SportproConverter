using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.GUI;
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

            ICategoriesForm view = new CategoriesForm();
            CategoriesPresenter presenter = new CategoriesPresenter(view);


            // если совсем ничего не совпало, предлагаем пользователю выбрать вручную
            if (maxMatchCount == 0)
            {
                view.LoadTree(CatalogDictionary.Instance.Catalog);
                if (view.ShowDialog() != DialogResult.OK)
                {
                    // ниче не выбрали
                    return "";
                }

                return view.SelectedCategory.PluginExportString;
            }
            var bestList = CatalogDictionary.Instance.AllCategoriesList.Where(c => c.MatchCount == maxMatchCount).ToList();

            if (bestList.Count > 1)
            {
                // у нас несколько победителей. Пускай пользователь решает что с ними делать
                view.LoadTree(CatalogDictionary.Instance.Catalog);
                bestList.ForEach(c =>
                {
                    view.HighlightNode(c, Color.Firebrick);
                    view.ExpandToNode(c);
                });

                view.SelectedCategory = bestList[0];

                if (view.ShowDialog() != DialogResult.OK)
                {
                    // ниче не выбрали
                    return "";
                }

                return view.SelectedCategory.PluginExportString;
            }
            return bestList[0].PluginExportString;

            //var newList = AllCategoriesList.OrderByDescending(c => c.MatchCount(categoriesList)).ToList();

        }

    }
}
