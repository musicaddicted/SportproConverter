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

        public Catalog Catalog { get; private set; }

        public CatalogDictionary()
        {
            Init();
        }

        private void Init()
        {
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer =
            new XmlSerializer(typeof(Catalog));
            // To read the file, create a FileStream.
            FileStream myFileStream =
            new FileStream(FilePath, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            Catalog = (Catalog) mySerializer.Deserialize(myFileStream);
            myFileStream.Close();

            // filling tags
            Catalog.Categories.ForEach(c => c.FillTags(c.Categories));
            AllCategoriesList = new List<Category>();
            Catalog.Categories.ForEach(c => AllCategoriesList.AddRange(c.GetChildren(c.Categories)));
        }



        public List<Category> AllCategoriesList { get; private set; }
    }
}
