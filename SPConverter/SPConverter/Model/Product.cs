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
        /// Бренд товара
        /// </summary>
        public string Brand;

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
        /// Остатки
        /// </summary>
        public List<Remain> Remains;

        /// <summary>
        /// Цвет
        /// </summary>
        public string Color;

        /// <summary>
        /// Общее кол-во остатков на складе
        /// </summary>
        public int RemainsTotalCount
        {
            get
            {
                return Remains?.Sum(r => r.Quantity) ?? 0;
            }
        }

        public string DefaultAttribute
        {
            get
            {
                if (Remains == null)
                    return "";
                if (Remains.Any(r => r.Size.Contains("M")))
                    return "M";

                return "";
            } 
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Остаток на складе
    /// </summary>
    public class Remain
    {
        public string Size;
        public int Quantity;
        public string Price;

        public override string ToString()
        {
            return Size;
        }
    }
}
