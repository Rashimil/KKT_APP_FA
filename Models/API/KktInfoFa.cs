using KKT_APP_FA.Enums;
using KKT_APP_FA.Extensions;
using KKT_APP_FA.Helpers;
using KKT_APP_FA.Models.KKTResponse;
using KKT_APP_FA.Services.Helpers;
using KKT_APP_FA.StaticValues;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Models.API         //[Description("Заводской номер ККТ")]
{
    // Информация о ККТ, подогнанная под ФА. Возврашается в таком виде к AdminApp. 
    // Требует доработки на стороне AdminApp
    public class KktInfoFa
    {
        FlagsHelper flagsHelper;
        ReflectionHelper reflectionHelper;
        public KktInfoFa()
        {
            this.flagsHelper = new FlagsHelper();
            this.reflectionHelper = new ReflectionHelper();
        }

        //==============================================================================================================================================

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

        // Установка версии ПО ККТ (0x03)
        public void Set0x03(GetFirmwareVersionResponse r)
        {
            this.FirmwareVersion = r.FirmwareVersion;
        }

        // Установка модели ККТ (0x04)
        public void Set0x04(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length > 0)
            {
                this.KKTModel = logicLevel.ConvertFromByteArray.ToString(DATA.XReverse().ToArray());
            }
        }

        // Установка заводского номера ФН (0x05) 
        public void Set0x05(GetFnNumberResponse r)
        {
            this.FN = r.FN;
        }

        // Установка версии ПО ФН (0x06)
        public void Set0x06(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length > 0)
            {
                this.FNFirmwareVersion = logicLevel.ConvertFromByteArray.ToString(DATA.XReverse().ToArray());
            }
        }

        // Установка срока действия ФН (0x07)
        public void Set0x07(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 5)
            {
                DATA = DATA.XReverse().ToArray(); // на всякий тут
                string yyyy = (2000 + logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[0] })).ToString();
                string mm = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[1] }).ToString();
                string dd = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[2] }).ToString();
                this.FNValidityTime = Fix(dd) + "." + Fix(mm) + "." + yyyy;
                this.ReRegistrationsAvialable = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[3] });
                this.ReRegistrationsCount = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[4] });
            }
        }

        // Установка статуса ФН (0x08)
        public void Set0x08(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 27)
            {
                DATA = DATA.XReverse().ToArray(); // на всякий тут
                this.CurrentDocument = DATA[1];
                this.CurrentDocumentDescription = EnumHelper.GetTypeDescription((CurrentDocumentEnum)CurrentDocument);
                if (DATA[2] == 0) { this.DocumentDataReceived = false; } else { this.DocumentDataReceived = true; }
                if (DATA[3] == 0) { this.ShiftOpened = false; } else { this.ShiftOpened = true; }
                this.FNFlags = DATA[4];
                //this.FNFlags = 0x7;
                if (FNFlags == (byte)FNFlagsEnum.None)
                {
                    this.FNFlagsDescription = EnumHelper.GetTypeDescription((FNFlagsEnum)FNFlags);
                }
                else
                {
                    byte a = FNFlags;
                    foreach (var item in Enum.GetValues(typeof(FNFlagsEnum))) // цикл по полям enum
                    {
                        if ((FNFlagsEnum)item != FNFlagsEnum.None)
                        {
                            var b = (byte)(FNFlagsEnum)item;
                            int r = a & b; // 
                            if (r != 0 && r == b) // если есть пересечение и результат = item
                            {
                                this.FNFlagsDescription += (EnumHelper.GetTypeDescription((FNFlagsEnum)item) + ", ");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(FNFlagsDescription) && FNFlagsDescription.Length >= 2)
                        FNFlagsDescription = FNFlagsDescription.Remove(FNFlagsDescription.Length - 2);
                }
                this.LastDocumentDateTime = logicLevel.ConvertFromByteArray.ToDateTime(DATA.Skip(5).Take(5).ToArray()).ToString("dd.MM.yyyy HH:mm:ss");
            }
            string yyyy = (2000 + logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[5] })).ToString();
            string mm = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[6] }).ToString();
            string dd = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[7] }).ToString();
            string hour = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[8] }).ToString();
            string min = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[9] }).ToString();
            this.LastDocumentDateTime = Fix(dd) + "." + Fix(mm) + "." + yyyy + " " + hour + ":" + Fix(min) + ":00";
            this.LastFD = logicLevel.ConvertFromByteArray.ToInt(DATA.Skip(26).XReverse().ToArray()).ToString();
        }

        // Установка текущих параметров регистрации ККТ (0x0A)
        public void Set0x0A(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 35)
            {
                DATA = DATA.XReverse().ToArray(); // на всякий тут
                this.KKTRegistrationNumber = logicLevel.ConvertFromByteArray.ToString(DATA.Take(20).ToArray());
                this.INN = logicLevel.ConvertFromByteArray.ToString(DATA.Skip(20).Take(12).ToArray());
                this.KKTOperatingMode = DATA.Skip(32).Take(1).ToArray()[0];
                byte a = KKTOperatingMode;
                foreach (var item in Enum.GetValues(typeof(KKTOperatingModeEnum))) // цикл по полям enum
                {

                    var b = (byte)(KKTOperatingModeEnum)item;
                    int r = a & b; // 
                    if (r != 0 && r == b) // если есть пересечение и результат = item
                    {
                        this.KKTOperatingModeDescription += (EnumHelper.GetTypeDescription((KKTOperatingModeEnum)item) + ", ");
                    }
                }
                if (!string.IsNullOrEmpty(KKTOperatingModeDescription) && KKTOperatingModeDescription.Length >= 2)
                    KKTOperatingModeDescription = KKTOperatingModeDescription.Remove(KKTOperatingModeDescription.Length - 2);

                this.TaxType = DATA.Skip(33).Take(1).ToArray()[0];
                a = TaxType;
                foreach (var item in Enum.GetValues(typeof(TaxTypeEnum))) // цикл по полям enum
                {

                    var b = (byte)(TaxTypeEnum)item;
                    int r = a & b; // 
                    if (r != 0 && r == b) // если есть пересечение и результат = item
                    {
                        this.TaxTypesDescription += (EnumHelper.GetTypeDescription((TaxTypeEnum)item) + ", ");
                    }
                }
                if (!string.IsNullOrEmpty(TaxTypesDescription) && TaxTypesDescription.Length >= 2)
                    TaxTypesDescription = TaxTypesDescription.Remove(TaxTypesDescription.Length - 2);

                this.AgentType = DATA.Skip(34).Take(1).ToArray()[0];
                if (AgentType == (byte)AgentEnum.None)
                {
                    this.AgentTypeDescription = EnumHelper.GetTypeDescription((AgentEnum)AgentType);
                }
                else
                {
                    a = AgentType;
                    foreach (var item in Enum.GetValues(typeof(AgentEnum))) // цикл по полям enum
                    {

                        var b = (byte)(AgentEnum)item;
                        int r = a & b; // 
                        if (r != 0 && r == b) // если есть пересечение и результат = item
                        {
                            this.AgentTypeDescription += (EnumHelper.GetTypeDescription((AgentEnum)item) + ", ");
                        }
                    }
                    if (!string.IsNullOrEmpty(AgentTypeDescription) && AgentTypeDescription.Length >= 2)
                        AgentTypeDescription = AgentTypeDescription.Remove(AgentTypeDescription.Length - 2);
                }
            }
        }

        // (0x0B) Установка версии конфигурации ККТ
        public void Set0x0B(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length > 0)
            {
                DATA = DATA.XReverse().ToArray(); // на всякий тут
                this.KKTConfigurationVersion = logicLevel.ConvertFromByteArray.ToString(DATA);
            }
        }

        // (0x0E) Установка текущих параметров TCP/IP
        public void Set0x0E(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 12)
            {
                DATA = DATA.XReverse().ToArray(); // на всякий тут
                //this.KKTIP = logicLevel.ConvertFromByteArray.ToInt(DATA.Take(4).ToArray()).ToString();

                this.KKTIP =
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[0] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[1] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[2] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[3] }).ToString();
                this.KKTNetworkMask = 
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[4] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[5] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[6] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[7] }).ToString();
                this.KKTGateWay =
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[8] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[9] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[10] }).ToString() + "." +
                    logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[11] }).ToString();
            }
        }

        // (0x50) Установка статуса информационного обмена с ОФД
        public void Set0x50(LogicLevel logicLevel)
        {
            var DATA = logicLevel.response.DATA;
            if (DATA != null && DATA.Length >= 13)
            {
                DATA = DATA.XReverse().ToArray(); // на всякий тут
                this.InformationExchangeStatus = DATA[0];
                this.OFDMessageReadingStatus = DATA[1];
                this.OFDMessageCount = logicLevel.ConvertFromByteArray.ToShort(DATA.Skip(2).Take(2).ToArray());
                this.OFDFirstDocumentNumber = logicLevel.ConvertFromByteArray.ToInt(DATA.Skip(4).Take(4).ToArray());
                string yyyy = (2000 + logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[8] })).ToString();
                string mm = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[9] }).ToString();
                string dd = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[10] }).ToString();
                string hour = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[11] }).ToString();
                string min = logicLevel.ConvertFromByteArray.ToByte(new byte[] { DATA[12] }).ToString();
                this.OFDFirstDocumentDateTime = Fix(dd) + "." + Fix(mm) + "." + yyyy + " " + hour + ":" + Fix(min) + ":00";
                if (OFDFirstDocumentDateTime == "00.00.2000 0:00:00")
                {
                    this.OFDFirstDocumentDateTime = "";
                }
            }
        }

        // (0x3B), Установка данных отчета о регистрации по всем тэгам
        public void set0x3B()
        {
            this.kktRegistrationReport = KktStaticValues.kktRegistrationReport; // берем со статики
        }

        //==============================================================================================================================================

        // (0x01)
        public string KKTFactoryNumber { get; set; } // заводской номер ККТ
        public string KKTDateTime { get; set; } // Дата и время ККТ // ToString(yyyyMMddHHmmss)
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
        public string CurrentDocumentDescription { get; set; } // Текущий документ (описание) (CurrentDocumentEnum)
        public bool DocumentDataReceived { get; set; } // Данные документа. В оригинале byte
        public bool ShiftOpened { get; set; } // Данные смены. В оригинале byte

        public byte FNFlags { get; set; } // флаги состояния ФН (FNFlagsEnum)
        public string FNFlagsDescription { get; set; } //  флаги состояния ФН (описание). Может быть несколько, надо проверять через логическое умножение с FNFlagsEnum (см тестовое приложение) 

        //BitArray bitArr = new BitArray(BitConverter.GetBytes(val));

        public string LastDocumentDateTime { get; set; } // 
        public string FN { get; set; } // номер ФН
        public string LastFD { get; set; } // Номер последнего ФД

        // (0x09) Описание отсутствует в доке

        // (0x0A) Запрос текущих параметров регистрации ККТ (можно номер документа брать равным полю ReRegistrationsCount)
        public string KKTRegistrationNumber { get; set; } // РН ККТ. Дополняется пробелами справа до длины 20 символов
        public string INN { get; set; } // ИНН. Дополняется пробелами справа до длины 12 символов
        public byte KKTOperatingMode { get; set; } // Режимы работы ККТ. Временно byte. Битовая маска
        public string KKTOperatingModeDescription { get; set; } // Описание режимов работы ККТ. М/б несколько
        public byte TaxType { get; set; } // Режимы налогообложения. Битовая маска
        public string TaxTypesDescription { get; set; } // Режимы налогообложения (расшифровка). Может быть несколько, надо проверять через логическое умножение с TaxTypEnum
        public byte AgentType { get; set; } // Признак платежного агента. Битовая маска
        public string AgentTypeDescription { get; set; } // Признак платежного агента (расшифровка). Может быть несколько, надо проверять через логическое умножение с AgentEnum

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

        // 0x3B Отчет о регистрации ККТ (по всем тэгам):
        public KktRegistrationReport kktRegistrationReport { get; set; } 

        //==============================================================================================================================================

        // Служебный метод для вывода красивых дат
        private string Fix(string num)
        {
            if (num.Length == 1)
            {
                num = "0" + num;
            }
            return num;
        }
    }
}
