using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.API
{
    public partial class KktInfo
    {
        // для красивости в названиях файлов :3
    }
    public class LibfptrFndtRegInfo // Регистрационные данные
    {
        [Display(Name = "Список систем налогообложения: ")]
        public int taxationTypes { get; set; }
        [Display(Name = "Признак агента: ")]
        public int agentSign { get; set; }
        [Display(Name = "Версия ФФД : ")]
        public int ffdVersion { get; set; }
        [Display(Name = "Признак автоматического режима: ")]
        public bool autoModeSign { get; set; }
        [Display(Name = "Признак автономного режима: ")]
        public bool offlineModeSign { get; set; }
        [Display(Name = "Признак шифрования: ")]
        public bool encryptionSign { get; set; }
        [Display(Name = "Признак ККТ для расчетов в сети Интернет: ")]
        public bool internetSign { get; set; }
        [Display(Name = "Признак расчетов за услуги: ")]
        public bool serviceSign { get; set; }
        [Display(Name = "Признак АС БСО: ")]
        public bool bsoSign { get; set; }
        [Display(Name = "Признак проведения лотерей: ")]
        public bool lotterySign { get; set; }
        [Display(Name = "Признак проведения азартных игр: ")]
        public bool gamblingSign { get; set; }
        [Display(Name = "Признак подакцизного товара: ")]
        public bool exciseSign { get; set; }
        [Display(Name = "Признак установки принтера в автомате: ")]
        public bool machineInstallationSign { get; set; }
        [Display(Name = "Адрес сайта ФНС: ")]
        public string fnsUrl { get; set; }
        [Display(Name = "ИНН организации: ")]
        public string organizationVATIN { get; set; }
        [Display(Name = "Наименование организации: ")]
        public string organizationName { get; set; }
        [Display(Name = "Email организации: ")]
        public string organizationEmail { get; set; }
        [Display(Name = "Адрес места расчетов: ")]
        public string paymentsAddress { get; set; }
        [Display(Name = "Рег. номер: ")]
        public string registrationNumber { get; set; }
        [Display(Name = "Номер автоматического устройства: ")]
        public string machineNumber { get; set; }
        [Display(Name = "ИНН ОФД: ")]
        public string ofdVATIN { get; set; }
        [Display(Name = "Наименование ОФД: ")]
        public string ofdName { get; set; }
    }

    public class LibfptrFndtOfdExchangeStatus // Статус информационного обмена с ОФД
    {
        [Display(Name = "Статус обмена: ")]
        public int exchangeStatus { get; set; }
        [Display(Name = "Количество неотправленных документов: ")]
        public int unsentCount { get; set; }
        [Display(Name = "Номер первого неотправленного документа: ")]
        public int firstUnsentNumber { get; set; }
        [Display(Name = "Наличие сообщения для ОФД: ")]
        public bool ofdMessageRead { get; set; }
        [Display(Name = "Дата и время первого неотправленного документа: ")]
        public DateTime dateTime { get; set; }
    }

    public class LibfptrFndtFnInfo // Информация и статус ФН
    {
        [Display(Name = "Серийный номер ФН: ")]
        public string fnSerial { get; set; }
        [Display(Name = "Версия ФН: ")]
        public string fnVersion { get; set; }
        [Display(Name = "Тип ФН: ")]
        public int fnType { get; set; }
        [Display(Name = "Состояние ФН: ")]
        public int fnState { get; set; }
        [Display(Name = "Нерасшифрованный байт флагов ФН: ")]
        public int fnFlags { get; set; }
        [Display(Name = "Требуется срочная замена ФН: ")]
        public bool nNeedReplacement { get; set; }
        [Display(Name = "Исчерпан ресурс ФН: ")]
        public bool nExhausted { get; set; }
        [Display(Name = "Память ФН переполнена: ")]
        public bool nMemoryOverflow { get; set; }
        [Display(Name = "Превышено время ожидания ответа от ОФД: ")]
        public bool nOfdTimeout { get; set; }
        [Display(Name = "Критическая ошибка ФН: ")]
        public bool fnCriticalError { get; set; }
    }

    public class LibfptrFndtLastRegistration // Информация о последней регистрации / перерегистрации
    {
        [Display(Name = "Номер документа: ")]
        public int documentNumber { get; set; }
        [Display(Name = "Номер регистрации/перерегистрации: ")]
        public int registrationsCount { get; set; }
        [Display(Name = "Дата и время документа: ")]
        public DateTime dateTime { get; set; }
    }

    public class LibfptrFndtLastReceipt // Информация о последнем чеке
    {
        [Display(Name = "Номер документа: ")]
        public int documentNumber { get; set; }
        [Display(Name = "Тип чека: ")]
        public int receiptType { get; set; }
        [Display(Name = "Сумма чека: ")]
        public double receiptSum { get; set; }
        [Display(Name = "Фискальный признак документа: ")]
        public string fiscalSign { get; set; }
        [Display(Name = "Дата и время документа: ")]
        public DateTime dateTime { get; set; }
    }

    public class LibfptrFndtLastDocument // Информация о последнем документе
    {
        [Display(Name = "Номер документа: ")]
        public int documentNumber { get; set; }
        [Display(Name = "Фискальный признак документа: ")]
        public string fiscalSign { get; set; }
        [Display(Name = "Дата и время документа: ")]
        public DateTime dateTime { get; set; }
    }

    public class LibfptrFndtShift // Информация о смене
    {
        [Display(Name = "Количество чеков за смену: ")]
        public int receiptNumber { get; set; }
        [Display(Name = "Номер смены: ")]
        public int shiftNumber { get; set; }
    }

    public class LibfptrFndtDocumentsCountInShift // Количество ФД за смену
    {
        [Display(Name = "Количество ФД за смену: ")]
        public int documentsCount { get; set; }
    }

    public class LibfptrFndtFfdVersions // Версии ФФД
    {
        [Display(Name = "Версия ФФД устройства: ")]
        public int deviceFfdVersion { get; set; }
        [Display(Name = "Версия ФФД фискального накопителя: ")]
        public int fnFfdVersion { get; set; }
        [Display(Name = "Максимальная поддерживаемая версия ФФД устройства: ")]
        public int maxFfdVersion { get; set; }
        [Display(Name = "Минимальная поддерживаемая версия ФФД устройства: ")]
        public int minFfdVersion { get; set; }
        [Display(Name = "Применяемая версия ФФД для документов: ")]
        public int ffdVersion { get; set; }
    }

    public class LibfptrFndtValidity // Срок действия ФН
    {
        [Display(Name = "Осталось перерегистраций: ")]
        public int registrationsRemain { get; set; }
        [Display(Name = "Сделано перерегистраций: ")]
        public int registrationsCount { get; set; }
        [Display(Name = "Срок действия ФН: ")]
        public DateTime dateTime { get; set; }
    }

    public class LibfptrFndtErrors // Ошибки обмена с ОФД
    {
        [Display(Name = "Код ошибки сети: ")]
        public int networkError { get; set; }
        [Display(Name = "Текст ошибки сети: ")]
        public string networkErrorText { get; set; }
        [Display(Name = "Код ошибки ОФД: ")]
        public int ofdError { get; set; }
        [Display(Name = "Текст ошибки ОФД: ")]
        public string ofdErrorText { get; set; }
        [Display(Name = "Код ошибки ФН: ")]
        public int fnError { get; set; }
        [Display(Name = "Текст ошибки ФН: ")]
        public string fnErrorText { get; set; }
        [Display(Name = "Номер ФД, на котором произошла ошибка: ")]
        public int documentNumber { get; set; }
        [Display(Name = "Команда ФН, на которой произошла ошибка: ")]
        public int commandCode { get; set; }
    }

}
