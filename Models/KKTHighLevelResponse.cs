using KKT_APP_FA.Models.API;
using KKT_APP_FA.Models.KKTResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models
{
    // Ответ от KKTHighLevel. Инкапсулирует все ответы от класса KKT
    public class KKTHighLevelResponse
    {
        //=======================================================================================================================================
        public KKTHighLevelResponse()
        {
            this.error = false;
            this.SendCheckPosition = new List<BaseResponse>();
        }

        //=======================================================================================================================================
        public bool error { get; set; } // общая ошибка. Если true то в каком то из подполей ошибка, ее надо разбирать
        public decimal total { get; set; }
        public string fn_number { get; set; }
        public short shift_number { get; set; }
        public string receipt_datetime { get; set; }
        public int fiscal_receipt_number { get; set; }
        public int fiscal_document_number { get; set; }
        public string ecr_registration_number { get; set; }
        public long fiscal_document_attribute { get; set; }
        public string daemon_code { get; set; }
        public string device_code { get; set; }
        public int error_code { get; set; }
        public string error_text { get; set; }
        public string error_type { get; set; }

        // доп. свойства для совместимости с оранжем:
        public string ofd_name { get; set; }
        public string serial_number { get; set; } // серийный номер ККТ
        public string ofd_site { get; set; }
        public string ofd_inn { get; set; }
        public string cashier_name { get; set; }
        public string sender_email { get; set; }
        public double change { get; set; } // сдача
        public string fns_site { get; set; }

        //=======================================================================================================================================

        public GetShiftInfoResponse GetShiftInfo { get; set; }
        //public BaseResponse OpenShiftBegin { get; set; }
        //public BaseResponse SendCashierData { get; set; }
        public OpenShiftResponse OpenShift { get; set; }
        //public BaseResponse CloseShiftBegin { get; set; }
        public CloseShiftResponse CloseShift { get; set; }
        public BaseResponse CancelFiscalDocument { get; set; }
        public BaseResponse OpenCheck { get; set; }
        public List<BaseResponse> SendCheckPosition { get; set; }
        public BaseResponse SendPaymentData { get; set; }
        public BaseResponse SendAutomaticDeviceData { get; set; }
        public RegisterCheckResponse RegisterCheck { get; set; }
        public BaseResponse OpenCorrectionCheck { get; set; }
        public BaseResponse SendCorrectionCheckData { get; set; }
        public BaseResponse SendCorrectionAutomaticDeviceData { get; set; }
        public RegisterCorrectionCheckResponse RegisterCorrectionCheck { get; set; }
        //public GetFnNumberResponse GetFnNumber { get; set; }
        //public GetKktStatusResponse GetKktStatus { get; set; }
        //public GetRegistrationParametersResponse GetRegistrationParameters { get; set; }
        //public GetFirmwareVersionResponse GetFirmwareVersion { get; set; }

        //=======================================================================================================================================

        public KktInfoFa KktInfoFa; // ответ на GetKktInfo

        //=======================================================================================================================================
    }
}
