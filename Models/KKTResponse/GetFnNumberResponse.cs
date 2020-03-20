using KKT_APP_FA.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.KKTResponse
{
    public class GetFnNumberResponse : BaseResponse
    {
        public GetFnNumberResponse(LogicLevel logicLevel) : base(logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length == 16)
            {
                this.FN = logicLevel.ConvertFromByteArray.ToString(DATA).ToString();
            }
        }

        public string FN { get; set; }
    }
}
