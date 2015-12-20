using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPConverter
{
    public static class Extensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list) => list == null || !list.Any();
    }
}
