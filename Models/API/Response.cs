using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.API
{
    public class Response // Ответ на входящий запрос
    {
        public string group_code { get; set; }
        public string operation { get; set; } // Операция. Варианты: registration (все), correction (все), openShift, closeShift
        public string uuid { get; set; }
        public string timestamp { get; set; }
        public bool error { get; set; }
        public object body { get; set; } // тело запроса. Варианты: Registration (все 4), Correction (все 2), OpenShift, CloseShift (Классы чеков)
        public string crc { get; set; }
        public string kkt_id { get; set; } // id KKT. Используется при необходимости отправки ответа о статусе ккт и тд (НУЖНО!!!)


        // доп. свойства для совместимости с оранжем:
        public string ofd_name { get; set; }
        public string serial_number { get; set; } // серийный номер ККТ
        public string ofd_site { get; set; }
        public string ofd_inn { get; set; }
        public string cashier_name { get; set; }
        public string sender_email { get; set; }
        public double change { get; set; } // сдача
    }

    //=======================================================================================================================================

    public class ResponseOK // body успешного ответа от ККТ 
    {
        public string uuid { get; set; }
        public object error { get; set; }
        public string status { get; set; }
        public Payload payload { get; set; }
        public string timestamp { get; set; }
        public string group_code { get; set; }
        public string daemon_code { get; set; }
        public string device_code { get; set; }
        public string callback_url { get; set; }
    }

    public class Payload
    {
        public double total { get; set; }
        public string fns_site { get; set; }
        public string fn_number { get; set; }
        public int shift_number { get; set; }
        public string receipt_datetime { get; set; }
        public int fiscal_receipt_number { get; set; }
        public int fiscal_document_number { get; set; }
        public string ecr_registration_number { get; set; }
        public long fiscal_document_attribute { get; set; }
    }

    //=======================================================================================================================================

    public class ResponseError // body шибочного ответа от ККТ
    {
        public Error error { get; set; }
        public string status { get; set; }
        public object payload { get; set; }
        public string timestamp { get; set; }
        public string callback_url { get; set; }
    }

    public class Error
    {
        public string error_id { get; set; }
        public int code { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }

    //=======================================================================================================================================
}
