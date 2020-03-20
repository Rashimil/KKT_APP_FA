using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTResponse
{
    public class GetRegistrationParametersResponse : BaseResponse
    {
        public GetRegistrationParametersResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 35)
            {
                this.KKTRegistrationNumber = logicLevel.ConvertFromByteArray.ToString(DATA.Take(20).XReverse().ToArray());
                this.INN = logicLevel.ConvertFromByteArray.ToString(DATA.Skip(20).Take(12).XReverse().ToArray());
                this.KKTOperatingMode = (DATA.Skip(32).Take(1).XReverse().ToArray())[0];
                this.TaxTypes = (DATA.Skip(33).Take(1).XReverse().ToArray())[0];
                this.AgentType = (DATA.Skip(34).Take(1).XReverse().ToArray())[0];
            }
        }

        public string KKTRegistrationNumber { get; set; } // РН ККТ. Дополняется пробелами справа до длины 20 символов
        public string INN { get; set; } // ИНН. Дополняется пробелами справа до длины 12 символов
        public byte KKTOperatingMode { get; set; } // Режимы работы ККТ. Временно byte. Битовая маска
        public byte TaxTypes { get; set; } // Режимы налогообложения. Временно byte. Битовая маска
        public byte AgentType { get; set; } // Признак платежного агента. Временно byte. Битовая маска
    }
}
