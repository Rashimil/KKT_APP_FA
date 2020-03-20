using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTRequest
{
    // Элемент запроса в ККТ (только для элементов TLV)
    public class KKTRequestProperty<T>
    {
        public int TAG { get; set; }
        public T USER_VALUE { get; set; }
    }
}
