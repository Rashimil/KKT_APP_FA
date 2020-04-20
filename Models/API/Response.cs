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

    public partial class KktInfo // body ответа информации о ККТ (увы, пока в формате АТОЛа, для ускорения разработки...)
    {
        public KktInfo()
        {
            libfptr_fndt_reg_info = new LibfptrFndtRegInfo();
            libfptr_fndt_ofd_exchange_status = new LibfptrFndtOfdExchangeStatus();
            libfptr_fndt_fn_info = new LibfptrFndtFnInfo();
            libfptr_fndt_last_registration = new LibfptrFndtLastRegistration();
            libfptr_fndt_last_receipt = new LibfptrFndtLastReceipt();
            libfptr_fndt_last_document = new LibfptrFndtLastDocument();
            libfptr_fndt_shift = new LibfptrFndtShift();
            libfptr_fndt_documents_count_in_shift = new LibfptrFndtDocumentsCountInShift();
            libfptr_fndt_ffd_versions = new LibfptrFndtFfdVersions();
            libfptr_fndt_validity = new LibfptrFndtValidity();
            libfptr_fndt_errors = new LibfptrFndtErrors();
        }
        public LibfptrFndtRegInfo libfptr_fndt_reg_info { get; set; } // Регистрационные данные
        public LibfptrFndtOfdExchangeStatus libfptr_fndt_ofd_exchange_status { get; set; } // Статус информационного обмена
        public LibfptrFndtFnInfo libfptr_fndt_fn_info { get; set; } // Информация о ФН
        public LibfptrFndtLastRegistration libfptr_fndt_last_registration { get; set; } // Информация о последней регистрации / перерегистрации
        public LibfptrFndtLastReceipt libfptr_fndt_last_receipt { get; set; } // Информация о последнем чеке
        public LibfptrFndtLastDocument libfptr_fndt_last_document { get; set; } // Информация о последнем фискальном документе
        public LibfptrFndtShift libfptr_fndt_shift { get; set; } // Информация о смене
        public LibfptrFndtDocumentsCountInShift libfptr_fndt_documents_count_in_shift { get; set; } // Количество ФД за смену
        public LibfptrFndtFfdVersions libfptr_fndt_ffd_versions { get; set; } // Версии ФФД
        public LibfptrFndtValidity libfptr_fndt_validity { get; set; } // Срок действия ФН
        public LibfptrFndtErrors libfptr_fndt_errors { get; set; } // Ошибки обмена с ОФД

    }
}
