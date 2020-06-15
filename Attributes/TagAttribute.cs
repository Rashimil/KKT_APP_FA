using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(int tag, Type originType = null)
        {
            this.Tag = tag;
            this.OriginType = originType;
        }
        public int Tag { get; }
        public Type OriginType { get; } // оригинальный тип, получаемый из ККТ в виде байтмассива (VALUE из TLV). Поля с этим атрибутом надо обрабатывать особо
    }
}
