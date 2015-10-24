using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPConverter.Model
{
    /// <summary>
    /// Товар (согласно плагину)
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Категории через ,
        /// </summary>
        public string Categories;

        /// <summary>
        /// Артикул
        /// </summary>
        public string Articul;

        /// <summary>
        /// Метки товара
        /// </summary>
        public string Tags;

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name;

        /// <summary>
        /// Полное описание
        /// </summary>
        public string FullDescription;

        /// <summary>
        /// Краткое описание
        /// </summary>
        public string ShortDescription;

        /// <summary>
        /// Цена
        /// </summary>
        public string Price;

        /// <summary>
        /// Цена со скидкой
        /// </summary>
        public string PriceWithSale;

        /// <summary>
        /// Количество
        /// </summary>
        public string Quantity;

        /// <summary>
        /// Цвет
        /// </summary>
        public string Color;

        /// <summary>
        /// Размер
        /// </summary>
        public string Size;

        
    }
}
