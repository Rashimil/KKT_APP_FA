using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKT_APP_FA.Enums;
using KKT_APP_FA.Helpers;

namespace KKT_APP_FA.Models.KKTResponse
{
    // Базовый класс ответа модуля ККТ
    public class BaseResponse
    {
        public BaseResponse(LogicLevel logicLevel)
        {
            if (logicLevel.response.RESULT != null && logicLevel.response.RESULT.Length != 0)
            {
                if (logicLevel.response.RESULT[0] == 0) // всё ок, поле DATA либо пустое либо содержит результат выполнения команды
                {
                    this.Result = logicLevel.response.RESULT[0];
                    this.ErrorCode = (ErrorCodeEnum)logicLevel.response.RESULT[0];
                }
                else // команда выполнена не была, поле DATA содержит код ошибки (1байт)
                {
                    this.Result = logicLevel.response.DATA[0];
                    this.ErrorCode = (ErrorCodeEnum)logicLevel.response.DATA[0];
                }

                this.Description = EnumHelper.GetTypeDescription(ErrorCode);
                foreach (var item in logicLevel.responseCommand)
                {
                    this.FAResponse += Convert.ToString(item, 16).PadLeft(2, '0').ToUpper() + " ";
                }
            }
            else
            {
                this.Result = (byte)ErrorCodeEnum.UnknownError;
                this.ErrorCode = (ErrorCodeEnum)Result;
                this.Description = EnumHelper.GetTypeDescription(ErrorCode);
            }
            this.FARequest = "";
            foreach (var item in logicLevel.requestCommand)
            {
                this.FARequest += Convert.ToString(item, 16).PadLeft(2, '0').ToUpper() + " ";
            }
        }
        public byte Result { get; set; }
        public ErrorCodeEnum ErrorCode { get; set; }
        public string Description { get; set; } // описание
        public string FARequest { get; set; } // запрос в ККТ
        public string FAResponse { get; set; } // Ответ от ККТ
    }
}
