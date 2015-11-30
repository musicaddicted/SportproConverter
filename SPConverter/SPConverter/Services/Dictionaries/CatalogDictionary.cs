using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SPConverter.Model;

namespace SPConverter.Services.Dictionaries
{
    /// <summary>
    /// Каталог сайта
    /// </summary>
    public class CatalogDictionary
    {
        private static CatalogDictionary _instance;

        public static CatalogDictionary Instance => _instance ?? (_instance = new CatalogDictionary());

        private static readonly string FilePath = Path.Combine(Global.Instance.RootDir, "Dictionaries", "catalog.xml");

        public Category Catalog { get; private set; }

        /// <summary>
        /// Категории в виде списка
        /// </summary>
        public List<Category> AllCategoriesList { get; private set; }

        public CatalogDictionary()
        {
            Init();
        }

        private void Init()
        {
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer =
            new XmlSerializer(typeof(Category));
            // To read the file, create a FileStream.
            FileStream myFileStream =
            new FileStream(FilePath, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            Catalog = (Category) mySerializer.Deserialize(myFileStream);
            myFileStream.Close();

            Catalog.Traverse(FillTagsAndParents);

            AllCategoriesList = new List<Category>();
            Catalog.Traverse(category => { AllCategoriesList.Add(category); });
        }

        //public List<Category> GetBestFitByName(List<string> categoriesList)
        //{
        //    var newList = AllCategoriesList.OrderByDescending(c => c.MatchCount(categoriesList)).ToList();

        //    //return newList;
        //    //Catalog.Traverse(category =>
        //    //{
        //    //    category.Tags.ForEach(t => Console.Write($"{t}>"));
        //    //    Console.WriteLine($" ({category.MatchCount(categoriesList)})");
        //    //});
        //}
        
        /// <summary>
        /// Рекурсивно заполняем теги, состоящие из названий категорий
        /// </summary>
        private void FillTagsAndParents(Category category)
        {
            category.Tags.Add(category.Name);
            category.PluginExportString += category.Name + ">";

            if (category.Categories.Count == 0)
                category.PluginExportString = category.PluginExportString.Remove(category.PluginExportString.Length - 1);

            category.Categories.ForEach(children =>
            {
                children.Tags.AddRange(category.Tags);
                children.Tags.ForEach(t => children.PluginExportString += t + ">");

                children.Parent = category;
            });
        }
        
    }
}
