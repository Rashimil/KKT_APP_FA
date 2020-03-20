using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTResponse
{
    public class GetFirmwareVersionResponse : BaseResponse
    {
        public GetFirmwareVersionResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null)
            {
                this.FirmwareVersion = logicLevel.ConvertFromByteArray.ToString(DATA.XReverse().ToArray()).ToString();
            }

        }
        public string FirmwareVersion { get; set; }
    }
}
