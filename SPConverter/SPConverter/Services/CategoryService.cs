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

        public CategoryService()
        {
            LastResult = CategoryChoiсeResult.Undefined;
        }

        public CategoryChoiсeResult ParseCategory(List<DinamoCategory> dinamoCategories)
        {
            string dinamoCategoriesList = "";
            dinamoCategories.Reverse();
            dinamoCategories.ForEach(d => dinamoCategoriesList += d.CleanName + ">");
            dinamoCategoriesList = dinamoCategoriesList.TrimEnd('>');

            string dinamoOriginalCategoriesList = "";
            dinamoCategories.ForEach(d => dinamoOriginalCategoriesList += d.OriginalName + ">");
            dinamoOriginalCategoriesList = dinamoOriginalCategoriesList.TrimEnd('>');

            LoadChoise(dinamoCategoriesList);
            if (LastResult != CategoryChoiсeResult.Undefined)
                return LastResult;

            

            CatalogDictionary.Instance.AllCategoriesList.ForEach(
                dictCat => dictCat.SetMatchCount(dinamoCategories.ConvertAll(c => c.CleanName)));

            var maxMatchCount = CatalogDictionary.Instance.AllCategoriesList.Max(c => c.MatchCount);

            ICategoriesForm view = new CategoriesForm();
            CategoriesPresenter presenter = new CategoriesPresenter(view, false);


            List<Category> bestList = new List<Category>();
            if (maxMatchCount != 0)
            {
                bestList =
                    CatalogDictionary.Instance.AllCategoriesList.Where(
                        c => maxMatchCount != 0 && c.MatchCount == maxMatchCount).ToList();

                if (bestList.Count == 1)
                {
                    ChosenCategoryString = bestList[0].PluginExportString;
                    LastResult = CategoryChoiсeResult.Choose;
                    return LastResult;
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
                        LastResult = CategoryChoiсeResult.Choose;
                        return LastResult;
                    case DialogResult.No:
                        // вроде как не надо такое сохранять
                        //if (view.SaveChoiсe)
                        //    WriteChoice(dinamoCategoriesList, CategoryChoiсeResult.Blank, null);
                        ChosenCategoryString = "";
                        LastResult = CategoryChoiсeResult.Blank;
                        return LastResult;
                    case DialogResult.Ignore:
                        if (view.SaveChoiсe)
                            WriteChoice(dinamoCategoriesList, CategoryChoiсeResult.Ignore, null);
                        LastResult = CategoryChoiсeResult.Ignore;
                        return CategoryChoiсeResult.Ignore;
                    default:
                        LastResult = CategoryChoiсeResult.Blank;
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
                ChoicesDictionary.Instance.Choices.Find(c => string.Equals(c.OriginalCategory, dinamoCategoriesList,StringComparison.InvariantCultureIgnoreCase));

            if (previousChoice == null)
            {
                LastResult = CategoryChoiсeResult.Undefined;
                return;
            }

            ChosenCategoryString = previousChoice.ResultCategory;

            if (string.IsNullOrEmpty(previousChoice.ResultCategory))
            {
                LastResult = CategoryChoiсeResult.Ignore;
                return;
            }

            LastResult = CategoryChoiсeResult.Choose;
        }

        public string ChosenCategoryString { get; set; }

        public CategoryChoiсeResult LastResult { get; set; }
    }
}
