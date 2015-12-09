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
            string dinamoCategoriesList = "";
            dinamoCategories.ForEach(d => dinamoCategoriesList += d.CleanName + ">");
            dinamoCategoriesList = dinamoCategoriesList.TrimEnd('>');

            string dinamoOriginalCategoriesList = "";
            dinamoCategories.ForEach(d => dinamoOriginalCategoriesList += d.OriginalName + ">");
            dinamoOriginalCategoriesList = dinamoOriginalCategoriesList.TrimEnd('>');

            CatalogDictionary.Instance.AllCategoriesList.ForEach(
                dictCat => dictCat.SetMatchCount(dinamoCategories.ConvertAll(c => c.CleanName)));

            var maxMatchCount = CatalogDictionary.Instance.AllCategoriesList.Max(c => c.MatchCount);

            ICategoriesForm view = new CategoriesForm();
            CategoriesPresenter presenter = new CategoriesPresenter(view);


            // если совсем ничего не совпало, предлагаем пользователю выбрать вручную
            if (maxMatchCount == 0)
            {
                view.HeaderAppendText("Выберите категорию для:\r\n", Color.Black);
                view.HeaderAppendText($"    {dinamoCategoriesList}\r\n", Color.DarkRed);
                view.HeaderAppendText($"    {dinamoOriginalCategoriesList}\r\n", Color.Salmon);
                view.HeaderAppendText($"Найдено подходящих категорий: 0", Color.Black);

                view.LoadTree(CatalogDictionary.Instance.Catalog);
                if (view.ShowDialog() != DialogResult.OK)
                {
                    // ниче не выбрали
                    return "";
                }

                LastChosenCategory = view.SelectedCategory;
                return view.SelectedCategory.PluginExportString;
            }
            var bestList = CatalogDictionary.Instance.AllCategoriesList.Where(c => c.MatchCount == maxMatchCount).ToList();

            if (bestList.Count > 1)
            {
                view.HeaderAppendText("Выберите категорию для:\r\n", Color.Black);
                view.HeaderAppendText($"    {dinamoCategoriesList}\r\n", Color.DarkRed);
                view.HeaderAppendText($"    {dinamoOriginalCategoriesList}\r\n", Color.Salmon);
                view.HeaderAppendText($"Найдено подходящих категорий: {bestList.Count}", Color.Black);
                //$"Выберите категорию для:\r\n <b>{dinamoCategoriesList}</b>\r\nИсходое название:{dinamoOriginalCategoriesList}\r\nНайдено подходящих категорий: {bestList.Count}";

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

                LastChosenCategory = view.SelectedCategory;
                return view.SelectedCategory.PluginExportString;
            }
            LastChosenCategory = bestList[0];
            return bestList[0].PluginExportString;

            //var newList = AllCategoriesList.OrderByDescending(c => c.MatchCount(categoriesList)).ToList();

        }

        public Category LastChosenCategory { get; set; }
    }
}
