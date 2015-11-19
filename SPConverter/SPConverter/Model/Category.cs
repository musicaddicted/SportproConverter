using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace SPConverter.Model
{

    // http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp

    /// <summary>
    /// Категория сайта
    /// </summary>
    [XmlRoot("Catalog")]
    public class Category
    {
        [XmlIgnore]
        public List<string> Tags = new List<string>();

        private List<Category> _children = new List<Category>();

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

        public List<Category> GetChildren(List<Category> children)
        {
            Categories.ForEach(cat =>
            {
                _children.Add(cat);
                cat.GetChildren(cat.Categories);
            });

            return children;
        }

        /// <summary>
        /// Возвращает кол-во вхождений тега каталога с сайта в слово-тег с остатков
        /// </summary>
        /// <param name="outTags"></param>
        /// <returns></returns>
        public int MatchCount(List<string> outTags)
        {
            return outTags.Count(outTag => Tags.Any(t => outTag.ToLower().Contains(t.ToLower())));
        }
    }
}