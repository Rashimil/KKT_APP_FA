using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTResponse
{
    public class RegisterCorrectionCheckResponse : BaseResponse
    {
        public RegisterCorrectionCheckResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 11)
            {
                this.CheckNumber = logicLevel.ConvertFromByteArray.ToShort(DATA.Take(2).XReverse().ToArray());
                this.FD = logicLevel.ConvertFromByteArray.ToUInt(DATA.Skip(2).Take(4).XReverse().ToArray()).ToString();
                this.FPD = logicLevel.ConvertFromByteArray.ToUInt(DATA.Skip(6).Take(4).XReverse().ToArray()).ToString();
                byte[] dt = DATA.Skip(10).Take(5).XReverse().ToArray();
            }
        }
        public short CheckNumber { get; set; } // Номер чека
        public string FD { get; set; } // Номер ФД
        public string FPD { get; set; } // Номер ФПД
    }
}
