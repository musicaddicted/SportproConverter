using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPConverter.Model
{
    /// <summary>
    /// Файл поставок
    /// </summary>
    public class Income
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; }

        public IncomeFileType Type { get; set; }

        public string FileName => Path.GetFileName(FilePath);

        /// <summary>
        /// Список всех позиций файла
        /// </summary>
        public List<Product> Products { get; set; }
    }
}
