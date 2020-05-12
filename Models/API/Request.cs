using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.API
{
    //=======================================================================================================================================

    public class Request // Входящий запрос
    {
        public string group_code { get; set; }
        public string operation { get; set; } // Операция. Варианты: registration (все 4), correction (все 2), openShift, closeShift
        public string uuid { get; set; }
        public string timestamp { get; set; }
        public object body { get; set; } // тело запроса. Варианты: Registration (все 4), Correction (все 2), OpenShift, CloseShift (Классы чеков)
        public string crc { get; set; }
        public string kkt_id { get; set; } // id KKT. Используется при необходимости отправки ответа о статусе ккт и тд (НАДО!!! )
    }

    //======================================================================================================================================= 

    public class Registration // body чеков расхода, прихода, возврат расхода и возврат прихода    
    {
        public string external_id { get; set; }
        public Receipt receipt { get; set; }
        public Service service { get; set; }
        public string timestamp { get; set; }
        public string authomat_number { get; set; } // номер автомата. ТОЛЬКО для ТерминалФА !!!
    }

    public class Client
    {
        public string email { get; set; } // 1008 Электронный адрес покупателя. Хотя бы одно обязательно (либо email либо phone)
        public string phone { get; set; } // 1008 Телефон покупателя. Хотя бы одно обязательно (либо email либо phone)
    }

    public class Company
    {
        public string email { get; set; } // 1117 Адрес электронной почты отправителя чека. Обязательное
        public string sno { get; set; } // 1055 Применяемая система налогообложения. Необязательно, если у организации один тип налогообложения
        public string inn { get; set; } // 1018 ИНН организации. Используется для предотвращения ошибочных регистраций чеков на ККТ. Обязательное
        public string payment_address { get; set; } // 1187 Место расчетов. Обязательное
    }

    public class Item
    {
        public string name { get; set; } // 1030 Наименование предмета расчета (товара). 
        public double price { get; set; } // 1079 Цена в рублях (2 знака после запятой)
        public double quantity { get; set; } //1023 Количество/вес (3 знака после запятой)
        public double sum { get; set; } // 1043 Сумма в рублях = 1023*1079 (2 знака после запятой)
        public string measurement_unit { get; set; } // 1197 Единица измерения предмета расчета
        public string payment_method { get; set; } // 1214 Признак способа расчета (full_prepaymen, prepayment и тд)
        public string payment_object { get; set; } // 1212 Признак предмета расчета (товар, услуга и тд)
        public string nomenclature_code { get; set; } // 1162 Код товарной номенклатуры (HEX с пробелами. Необязательно)
        public Vat vat { get; set; } // Налог на позицию (лучше передавать тут чем на весь чек)
        public Agent_info agent_info { get; set; } // Атрибуты агента. Необязательно. Слать только если агент
        public Supplier_info supplier_info { get; set; } // Атрибуты поставщика. Обязательно только если передан agent_Info
        public string user_data { get; set; } // 1191 Дополнительный реквизит предмета расчета. Необязательно
    }

    public class Agent_info
    {
        public string type { get; set; } // 1057 Признак агента (тип агента: банковский, простой, субагент и тд)
        public Paying_agent paying_agent { get; set; } // Атрибуты платежного агента.
        public Receive_payments_operator receive_payments_operator { get; set; } // Атрибуты оператора по приему платежей
        public Money_transfer_operator money_transfer_operator { get; set; } // Атрибуты оператора перевода      
    }

    public class Paying_agent
    {
        public string operation { get; set; } // 1044 Операция платежного агента
        public string[] phones { get; set; } // 1073 Телефоны платежного агента 
    }
    public class Receive_payments_operator
    {
        public string[] phones { get; set; } // 1074 Телефоны оператора по приему платежей
    }
    public class Money_transfer_operator
    {
        public string[] phones { get; set; } // 1075 Телефоны оператора перевода
        public string name { get; set; } // 1026 Наименование оператора перевода
        public string address { get; set; } // 1005 Адрес оператора перевода
        public string inn { get; set; } // 1016 Инн оператора перевода
    }

    public class Payment
    {
        public int type { get; set; } // Вид оплаты. 1-Электроными, 2-Аванс и тд
        public double sum { get; set; } // Сумма к оплате в рублях (2 знака после запятой)
    }

    public class Vat // Ставка налога       
    {
        public string type { get; set; } // none-без НДС, vat0-ставка 0% и тд
        public double sum { get; set; } // Сумма налога в рублях (2 знака после запятой) 
    }

    public class Receipt
    {
        public Client client { get; set; }
        public Company company { get; set; }
        public List<Item> items { get; set; } // А вдруг тут не массив???
        public List<Payment> payments { get; set; } // Оплаты
        public List<Vat> vats { get; set; } // Сумма налогов на весь чек. Лучше слать не тут, а отдельно на позицию
        public double total { get; set; } // Итоговая сумма чека в рублях (2 знака после запятой)
    }

    public class Supplier_info
    {
        public string[] phones { get; set; } // 1171 Телефоны поставщика
        public string name { get; set; } // 1225 Наименование поставщика
        public string inn { get; set; } // 1226 Инн оператора поставщика
    }

    public class Service
    {
        public string callback_url { get; set; }
    }

    //=======================================================================================================================================

    public class Correction // body чеков коррекции расхода, коррекции прихода
    {
        public string external_id { get; set; }
        public CorrectioN correction { get; set; }
        public Service service { get; set; }
        public string timestamp { get; set; }
        public string authomat_number { get; set; } // номер автомата. ТОЛЬКО для ТерминалФА !!!
    }

    public class CorrectioN
    {
        public Company company { get; set; }
        public CorrectionInfo correction_info { get; set; }
        public List<Payment> payments { get; set; }
        public List<Vat> vats { get; set; }
    }

    public class CorrectionInfo
    {
        public string type { get; set; }
        public string base_date { get; set; }
        public string base_number { get; set; }
        public string base_name { get; set; }
    }
    // классы Company, Payment, Vat, Service подходят от запроса Registration

    //=======================================================================================================================================
}
