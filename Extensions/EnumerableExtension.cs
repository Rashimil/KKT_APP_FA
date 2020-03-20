using KKT_APP_FA.Enums;
using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Extensions
{
    public static class EnumerableExtension // расширение класса Enumerable
    {
        /// <summary>
        /// Изменяет(или НЕ изменяет) порядок элементов в последовательности, в зависимости от аппаратной платформы
        /// </summary>
        /// <typeparam name="TSource">Тип элементов source.</typeparam>
        /// <param name="source">Последовательность значений, которые следует расставить в обратном порядке</param>
        /// <returns>Последовательность, элементы которой соответствуют элементам входной последовательности, но следуют в противоположном порядке (либо входную последовательность)</returns>
        /// 
        public static IEnumerable<TSource> XReverse<TSource>(this IEnumerable<TSource> source)
        {
            if (ByteOrderHelper.GetSystemByteOrder() == SystemByteOrderEnum.BE)
            {
                return source.Reverse();
            }
            else
            {
                return source;
            }
        }
    }
}
