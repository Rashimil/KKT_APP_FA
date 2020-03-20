using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Helpers;
using KKT_APP_FA.Extensions;

namespace KKT_APP_FA.Models.KKTResponse
{
    // Ответ на открытие смены
    public sealed class OpenShiftResponse : BaseResponse
    {
        public OpenShiftResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            byte[] DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length == 10)
            {
                var sn = DATA.Take(2).XReverse().ToArray();
                var fd = DATA.Skip(2).Take(4).XReverse().ToArray();
                var fpd = DATA.Skip(6).Take(4).XReverse().ToArray();
                this.ShiftNumber = logicLevel.ConvertFromByteArray.ToShort(sn);
                this.FD = logicLevel.ConvertFromByteArray.ToUInt(fd).ToString();
                this.FPD = logicLevel.ConvertFromByteArray.ToUInt(fpd).ToString();
            }
        }
        public short ShiftNumber { get; set; } // Номер открытой смены
        public string FD { get; set; } // Номер ФД
        public string FPD { get; set; } // Номер ФПД

        // FN брать из статичных данных
    }
}
