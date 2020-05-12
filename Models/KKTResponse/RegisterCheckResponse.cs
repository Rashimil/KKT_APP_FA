using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;

namespace KKT_APP_FA.Models.KKTResponse
{
    public class RegisterCheckResponse : BaseResponse
    {
        public RegisterCheckResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 17)
            {
                this.CheckNumber = logicLevel.ConvertFromByteArray.ToShort(DATA.Take(2).XReverse().ToArray());
                this.FD = logicLevel.ConvertFromByteArray.ToUInt(DATA.Skip(2).Take(4).XReverse().ToArray()).ToString();
                this.FPD = logicLevel.ConvertFromByteArray.ToUInt(DATA.Skip(6).Take(4).XReverse().ToArray()).ToString();
                byte[] dt = DATA.Skip(10).Take(5).XReverse().ToArray();
                this.CheckDateTime = logicLevel.ConvertFromByteArray.ToDateTime(dt).ToString("dd.MM.yyyy HH:mm:ss"); // ToString(yyyyMMddHHmmss)
                this.ShiftNumber = logicLevel.ConvertFromByteArray.ToShort(DATA.Skip(15).Take(2).XReverse().ToArray());
            }
        }
        public short CheckNumber { get; set; } // Номер чека
        public string FD { get; set; } // Номер ФД
        public string FPD { get; set; } // Номер ФПД
        public string CheckDateTime { get; set; } // Дата и время чека (до минуты) // ToString(yyyyMMddHHmmss)
        public short ShiftNumber { get; set; } // Номер  смены
    }
}
