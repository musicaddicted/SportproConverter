using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SPConverter.Model
{
    public partial class Category
    {
        [XmlIgnore]
        public Category Parent { get; set; }

        [XmlIgnore]
        public List<string> Tags = new List<string>();

        public void Traverse(Action<Category> action)
        {
            action(this);
            foreach (var child in Categories)
                child.Traverse(action);
        }

        [XmlIgnore]
        public int MatchCount;

        /// <summary>
        /// Список категорий в формате плагина (через '>')
        /// </summary>
        [XmlIgnore]
        public string PluginExportString;

        /// <summary>
        /// Возвращает кол-во вхождений тега каталога с сайта в слово-тег с остатков
        /// </summary>
        /// <param name="outTags"></param>
        /// <returns></returns>
        public void SetMatchCount(List<string> outTags)
        {
            MatchCount = outTags.Count(outTag => Tags.Any(t => outTag.ToLower().Contains(t.ToLower())));
        }
    }
}