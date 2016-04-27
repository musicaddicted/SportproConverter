using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SPConverter.Model;
using static System.String;

namespace SPConverter.Services.Dictionaries
{
    /// <summary>
    /// Каталог сайта
    /// </summary>
    public class CatalogDictionary
    {
        private static CatalogDictionary _instance;

        public static CatalogDictionary Instance => _instance ?? (_instance = new CatalogDictionary());

        private XmlDocument _xmlDocument;

        private XmlDocument XmlDocument
        {
            get
            {
                if (_xmlDocument != null)
                    return _xmlDocument;
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(FilePath);
                return _xmlDocument;
            }
        }

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
            using (FileStream myFileStream = new FileStream(FilePath, FileMode.Open))
            {
                Catalog = (Category) mySerializer.Deserialize(myFileStream);
                myFileStream.Close();
            }

            Catalog.Traverse(FillTagsAndParents);

            AllCategoriesList = new List<Category>();
            Catalog.Traverse(category => { AllCategoriesList.Add(category); });
        }

        /// <summary>
        /// Рекурсивно заполняем теги, состоящие из названий категорий
        /// </summary>
        private void FillTagsAndParents(Category category)
        {
            if (!IsNullOrEmpty(category.Name))
            {
                category.Tags.Add(category.Name);
                category.PluginExportString += category.Name;
            }

            if (category.Categories.Count == 0)
                category.PluginExportString = category.PluginExportString.Trim('>');

            category.Categories.ForEach(children =>
            {
                children.Tags.AddRange(category.Tags);
                children.Tags.ForEach(t =>
                {
                    if (!IsNullOrEmpty(t))
                    children.PluginExportString += t + ">";
                });

                children.Parent = category;
            });
        }

        public void RemoveNode(string xpathToNode)
        {
            var node = XmlDocument.SelectSingleNode(xpathToNode);
            node?.ParentNode.RemoveChild(node);

            Init();
        }
        
    }
}
