using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;
using KKT_APP_FA.Helpers;

namespace KKT_APP_FA.Models.KKTResponse
{
    // Ответ на GetShiftInfo
    public sealed class GetShiftInfoResponse : BaseResponse
    {
        public GetShiftInfoResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length == 5)
            {
                var shift = DATA[0];
                this.ShiftOpened = (shift == 0) ? false : true;
                //this.ShiftNumber = BitConverter.ToInt16(new byte[] { DATA[1], DATA[2] }, 0);
                //this.CheckNumber = BitConverter.ToInt16(new byte[] { DATA[3], DATA[4] }, 0);
                this.ShiftNumber = logicLevel.ConvertFromByteArray.ToShort(new byte[] { DATA[1], DATA[2] });
                this.CheckNumber = logicLevel.ConvertFromByteArray.ToShort(new byte[] { DATA[3], DATA[4] });
            }
            else
            {
                this.ShiftOpened = false;
            }
        }
        public bool ShiftOpened { get; set; } // false – смена закрыта, true – смена открыта 
        public short ShiftNumber { get; set; } // Если смена закрыта, то – номер последней закрытой смены, если открыта, то номер текущей смены
        public short CheckNumber { get; set; } // Если смена закрыта, то число документов в предыдущей закрытой смене. Если смена открыта, но нет ни одного чека, то 0. В остальных случаях номер последнего сформированного чека
    }
}
