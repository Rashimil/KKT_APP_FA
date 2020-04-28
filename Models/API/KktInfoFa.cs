using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using KKT_APP_FA.Models.KKTResponse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.API
{
    // Информация о ККТ, подогнанная под ФА. Возврашается в таком виде к AdminApp. 
    // Требует доработки на стороне AdminApp
    public class KktInfoFa    
    {
        // Установка статуса ККТ (0x01):
        public void Set0x01(GetKktStatusResponse r) 
        {
            this.KKTFactoryNumber = r.KKTFactoryNumber;
            this.KKTDateTime = r.KKTDateTime;
            this.HasCriticalError = r.HasCriticalError;
            this.PrinterStatus = r.PrinterStatus;
            this.FNConnected = r.FNConnected;
            this.FNLifePhase = r.FNLifePhase;
            this.FNLifePhaseDescription = r.FNLifePhaseDescription;
        }

        public void Set0x03(GetFirmwareVersionResponse r)
        {
            this.FirmwareVersion = r.FirmwareVersion;
        }



        //[Description("Заводской номер ККТ")]

        // (0x01)
        public string KKTFactoryNumber { get; set; } // заводской номер ККТ
        public string KKTDateTime { get; set; } // Дат и время ККТ // TODO ToString(yyyyMMddHHmmss)
        public bool HasCriticalError { get; set; } // Критические ошибки в ККТ. false – ошибок нет, true – присутствуют
        public byte PrinterStatus { get; set; } // 0 – Корректный статус, бумага присутствует, 1 – Устройство не подключено, 2 – Отсутствует бумага, 3 – Замятие бумаги, 5 – Открыта крышка ПУ, 6 – Ошибка отрезчика ПУ, 7 – Аппаратная ошибка ПУ
        public bool FNConnected { get; set; } // Наличие ФН в ККТ
        public byte FNLifePhase { get; set; } // Фаза жизни ФН 
        public string FNLifePhaseDescription { get; set; } // Фаза жизни ФН (описание) (FNLifePhaseEnum)

        // 0x02 (не нужно, дублируется с 0x01)

        // (0x03)
        public string FirmwareVersion { get; set; } // версия прошивки ККТ

        // (0x04)
        public string KKTModel { get; set; } // модель ККТ 

        // (0x05) (НЕ НУЖНО, ЕСТЬ В 0x08)
        // public string FN { get; set; } // номер ФН

        // (0x06)
        public string FNFirmwareVersion { get; set; } // версия ПО ФН

        // (0x07)
        public string FNValidityTime { get; set; } // срок действия ФН
        public byte ReRegistrationsAvialable { get; set; } // Доступно перерегистраций
        public byte ReRegistrationsCount { get; set; } // Проведено перерегистраций

        // 0x08 Запрос статуса ФН
        //public byte FNLifePhase { get; set; } // Фаза жизни ФН (FNLifePhaseEnum)  НЕ НУЖНО, ЕСТЬ В 0x01
        // public string FNLifePhaseDescription { get; set; } // Фаза жизни ФН (описание) (FNLifePhaseEnum)   НЕ НУЖНО, ЕСТЬ В 0x01
        public byte CurrentDocument { get; set; } // Текущий документ (CurrentDocumentEnum)
        public byte CurrentDocumentDescription { get; set; } // Текущий документ (описание) (CurrentDocumentEnum)
        public bool DocumentDataReceived { get; set; } // Данные документа. В оригинале byte
        public bool ShiftOpened { get; set; } // Данные смены. В оригинале byte

        public byte FNFlags { get; set; } // флаги состояния ФН (FNFlagsEnum)
        public string FNFlagsDescription { get; set; } //  флаги состояния ФН (описание). Может быть несколько, надо проверять через логическое умножение с FNFlagsEnum (см тестовое приложение) 

        //BitArray bitArr = new BitArray(BitConverter.GetBytes(val));

        public string LastDocumentDateTime { get; set; } // 
        public string FN { get; set; } // номер ФН
        public string LastFD { get; set; } // Номер последнего ФД

        // (0x09) Описание отсутствует в доке

        // (0x0A) Запрос текущих параметров регистрации ККТ
        public string KKTRegistrationNumber { get; set; } // РН ККТ. Дополняется пробелами справа до длины 20 символов
        public string INN { get; set; } // ИНН. Дополняется пробелами справа до длины 12 символов
        public byte KKTOperatingMode { get; set; } // Режимы работы ККТ. Временно byte. Битовая маска
        public byte TaxTypes { get; set; } // Режимы налогообложения. Битовая маска
        public byte TaxTypesDescription { get; set; } // Режимы налогообложения (расшифровка). Может быть несколько, надо проверять через логическое умножение с TaxTypEnum
        public byte AgentType { get; set; } // Признак платежного агента. Битовая маска
        public byte AgentTypeDescription { get; set; } // Признак платежного агента (расшифровка). Может быть несколько, надо проверять через логическое умножение с AgentEnum

        // (0x0B) Запрос версии конфигурации ККТ
        public string KKTConfigurationVersion { get; set; } //

        // 0x0E Запрос текущих параметров TCP/IP
        public string KKTIP { get; set; }
        public string KKTNetworkMask { get; set; }
        public string KKTGateWay { get; set; }

        // 0x50 Запрос статуса информационного обмена с ОФД
        public byte InformationExchangeStatus { get; set; } // Служебный параметр
        public byte OFDMessageReadingStatus { get; set; } // Служебный параметр
        public int OFDMessageCount { get; set; } // Количество сообщений для передачи в ОФД
        public int OFDFirstDocumentNumber { get; set; } // Номер первого в очереди документа для ОФД
        public string OFDFirstDocumentDateTime { get; set; } // Дата-время первого в очереди документа для ОФД

    }
}
