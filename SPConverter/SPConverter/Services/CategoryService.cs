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
        public enum CategoryChoiсeResult
        {
            Choose,
            Blank,
            Ignore,
            Undefined
        }

        public CategoryChoiсeResult ParseCategory(List<DinamoCategory> dinamoCategories)
        {
            string dinamoCategoriesList = "";
            dinamoCategories.ForEach(d => dinamoCategoriesList += d.CleanName + ">");
            dinamoCategoriesList = dinamoCategoriesList.TrimEnd('>');

            LoadChoise(dinamoCategoriesList);
            if (LastResult != CategoryChoiсeResult.Undefined)
                return LastResult;

            string dinamoOriginalCategoriesList = "";
            dinamoCategories.ForEach(d => dinamoOriginalCategoriesList += d.OriginalName + ">");
            dinamoOriginalCategoriesList = dinamoOriginalCategoriesList.TrimEnd('>');

            CatalogDictionary.Instance.AllCategoriesList.ForEach(
                dictCat => dictCat.SetMatchCount(dinamoCategories.ConvertAll(c => c.CleanName)));

            var maxMatchCount = CatalogDictionary.Instance.AllCategoriesList.Max(c => c.MatchCount);

            ICategoriesForm view = new CategoriesForm();
            CategoriesPresenter presenter = new CategoriesPresenter(view);


            List<Category> bestList = new List<Category>();
            if (maxMatchCount != 0)
            {
                bestList =
                    CatalogDictionary.Instance.AllCategoriesList.Where(
                        c => maxMatchCount != 0 && c.MatchCount == maxMatchCount).ToList();

                if (bestList.Count == 1)
                {
                    ChosenCategoryString = bestList[0].PluginExportString;
                    return CategoryChoiсeResult.Choose;
                }
            }

            // если совсем ничего не совпало или подходит несколько вариантов, предлагаем пользователю выбрать вручную
            if (maxMatchCount == 0 || bestList.Count > 1)
            {
                view.HeaderAppendText("Выберите категорию для:\r\n", Color.Black);
                view.HeaderAppendText($"    {dinamoCategoriesList}\r\n", Color.DarkRed);
                view.HeaderAppendText($"    {dinamoOriginalCategoriesList}\r\n", Color.Salmon);
                view.HeaderAppendText($"Найдено подходящих категорий: {bestList.Count}", Color.Black);

                view.LoadTree(CatalogDictionary.Instance.Catalog);

                bestList.ForEach(c =>
                {
                    view.HighlightNode(c, Color.Firebrick);
                    view.ExpandToNode(c);
                });

                if (!bestList.IsEmpty())
                    view.SelectedCategory = bestList[0];

                switch (view.ShowDialog())
                {
                    case DialogResult.OK:
                        if (view.SaveChoiсe)
                            WriteChoice(dinamoCategoriesList, CategoryChoiсeResult.Choose,
                                view.SelectedCategory.PluginExportString);
                        ChosenCategoryString = view.SelectedCategory.PluginExportString;
                        return CategoryChoiсeResult.Choose;
                    case DialogResult.No:
                        // вроде как не надо такое сохранять
                        //if (view.SaveChoiсe)
                        //    WriteChoice(dinamoCategoriesList, CategoryChoiсeResult.Blank, null);
                        ChosenCategoryString = "";
                        return CategoryChoiсeResult.Blank;
                    case DialogResult.Ignore:
                        if (view.SaveChoiсe)
                            WriteChoice(dinamoCategoriesList, CategoryChoiсeResult.Ignore, null);
                        return CategoryChoiсeResult.Ignore;
                    default:
                        return CategoryChoiсeResult.Blank;
                }
            }
            throw new Exception("Сюда мы не должны были попасть");
        }

        private void WriteChoice(string dinamoCategoriesList, CategoryChoiсeResult choose, string pluginExportString)
        {
            ChoicesDictionary.Instance.Add(new Choice {OriginalCategory = dinamoCategoriesList, ResultCategory = pluginExportString} );
        }

        private void LoadChoise(string dinamoCategoriesList)
        {
            var previousChoice =
                ChoicesDictionary.Instance.Choices.Find(c => c.OriginalCategory == dinamoCategoriesList);

            if (previousChoice == null)
            {
                LastResult = CategoryChoiсeResult.Undefined;
                return;
            }

            if (string.IsNullOrEmpty(previousChoice.ResultCategory))
            {
                LastResult = CategoryChoiсeResult.Blank;
                return;
            }

            ChosenCategoryString = previousChoice.ResultCategory;
            LastResult = CategoryChoiсeResult.Choose;
        }

        public string ChosenCategoryString { get; set; }

        public CategoryChoiсeResult LastResult { get; set; }
    }
}
