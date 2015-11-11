using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPConverter.Model
{
    public class DinamoCategory
    {
        /// <summary>
        /// Исходное наименование (e.g. 0110 Кроссовки волейбольные ASICS AW15)
        /// </summary>
        public string OriginalName;

        /// <summary>
        /// Очищенное наименование (e.g. Кроссовки волейбольные)
        /// </summary>
        public string CleanName;

        /// <summary>
        /// Первый блок до пробела
        /// </summary>
        public string FirstBlock => OriginalName.Contains(' ') ? OriginalName.Remove(OriginalName.IndexOf(' ')) : null;

        public override string ToString()
        {
            return CleanName;
        }
    }
}
