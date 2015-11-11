using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SPConverter.Model
{
    /// <summary>
    /// Каталог сайта
    /// </summary>
    public class Catalog
    {
        [XmlArray("Categories"), XmlArrayItem(typeof(Category), ElementName = "Category")]
        public List<Category> Categories;

    }
}