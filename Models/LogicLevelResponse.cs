using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;
using KKT_APP_FA.Helpers;

namespace KKT_APP_FA.Models
{
    // Ответы логического уровня (не всегда необходимы)
    public class LogicLevelResponse
    {
        public LogicLevelResponse()
        {
            this.connectResult = new TerminalConnectResult();
        }
        public void Set(ErrorCodeEnum errorCode)
        {
            this.Code = (byte)errorCode;
            this.Description = EnumHelper.GetTypeDescription(errorCode);
            this.Response = errorCode;
        }
        public byte Code { get; set; } // код 
        public string Description { get; set; } // описание
        public ErrorCodeEnum Response { get; set; } // сама ошибка
        public TerminalConnectResult connectResult; // результат соединения
    }
}
