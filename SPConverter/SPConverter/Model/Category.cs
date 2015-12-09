using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SPConverter.Model
{
    /// <summary>
    /// Категория сайта
    /// </summary>
    [XmlRoot("Catalog")]
    public partial class Category
    {
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Подкатегории
        /// </summary>
        [XmlArray("Categories"), XmlArrayItem(typeof(Category), ElementName = "Category")]
        public List<Category> Categories { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}