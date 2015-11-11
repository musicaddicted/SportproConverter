using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SPConverter.Model
{
    /// <summary>
    /// Категория сайта
    /// </summary>
    [XmlRoot("Catalog")]
    public class Category
    {
        [XmlIgnore]
        public List<string> Tags = new List<string>();

        [XmlAttribute]
        public string Name { get; set; }

        [XmlArray("Categories"), XmlArrayItem("Category")]
        public List<Category> Categories { get; set; }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Рекурсивно заполняем теги, состоящие из названий категорий
        /// </summary>
        /// <param name="children">Подкатегории</param>
        public void FillTags(List<Category> children)
        {
            Tags.Add(Name);

            if (Categories.Count == 0) return;
            Categories.ForEach(c =>
            {
                c.Tags.AddRange(Tags);
                c.FillTags(c.Categories);
            });
        }
    }
}