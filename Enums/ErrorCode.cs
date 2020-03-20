using System;
using System.ComponentModel;

namespace KKT_APP_FA.Enums
{
    //==============================================================================================================================================

    // Коды ошибок:
    public enum ErrorCodeEnum : byte
    {
        [Description("Успешно")]
        OK = 0x00,

        [Description("Неверный формат команды")]
        WrongFormat = 0x01,

        [Description("Данная команда требует другого состояния ФН")]
        WrongState = 0x02,

        [Description("Ошибка ФН")]
        FiscalStorageError = 0x03,

        [Description("Ошибка KC")]
        CryptoprocessorError = 0x04,

        [Description("Закончен срок эксплуатации ФН")]
        FiscalStorageLifeTime = 0x05,

        [Description("Архив ФН переполнен")]
        ArchiveIsFull = 0x06,

        [Description("Дата и время операции не соответствуют логике работы ФН")]
        WrongTime = 0x07,

        [Description("Запрошенные данные отсутствуют в Архиве ФН")]
        NoData = 0x08,

        [Description("Параметры команды имеют правильный формат, но их значение не верно")]
        ParametersWrongFormat = 0x09,

        [Description("Превышение размеров TLV данных")]
        TLVDataExceeding = 0x10,

        [Description("Исчерпан ресурс КС. Требуется закрытие фискального режима")]
        CryptoprocessorResourceExhausted = 0x12,

        [Description("Ресурс хранения документов для ОФД исчерпан")]
        OFDResourceExhausted = 0x14,

        [Description("Превышено время ожидания передачи сообщения (30 дней)")]
        WaitingTimeExceeded = 0x15,

        [Description("Продолжительность смены более 24 часов")]
        Session24 = 0x16,

        [Description("Неверная разница во времени между 2 операциями (более 5 минут)")]
        WrongTimeDifference = 0x17,

        [Description("Сообщение от ОФД не может быть принято")]
        MessageCanNotBeAccepted = 0x20,

        [Description("Неверная структура команды, либо неверная контрольная сумма")]
        WrongCommand = 0x25,

        [Description("Неизвестная команда")]
        UnknownCommand = 0x26,

        [Description("Неверная длина параметров команды")]
        WrongLength = 0x27,

        [Description("Неверный формат или значение параметров команды")]
        WrongParametersFormat = 0x28,

        [Description("Нет связи с ФН")]
        FiscalStorageNoConnect = 0x30,

        [Description("Неверные дата/время в ККТ")]
        WrongDateTime = 0x31,

        [Description("Переданы не все необходимые данные")]
        NotFullData = 0x32,

        [Description("РНМ сформирован неверно, проверка на данной ККТ не прошла")]
        WrongRN = 0x33,

        [Description("Данные уже были переданы ранее")]
        AlreadyTransferred = 0x34,

        [Description("Аппаратный сбой ККТ")]
        HardwareError = 0x35,

        [Description("Неверно указан признак расчета, возможные значения: приход, расход, возврат прихода, возврат расхода")]
        WrongCalculationSign = 0x36,

        [Description("Указанный налог не может быть применен")]
        WrongTax = 0x37,

        [Description("Данные необходимы только для платежного агента (указано при регистрации)")]
        DataForAhentOnly = 0x38,

        [Description("Итоговая сумма оплаты не равна стоимости предметов расчета")]
        WrongSum = 0x39,

        [Description("Сумма оплаты соответствующими типами (за исключением наличных) превышает итог чека")]
        LongSum = 0x3A,

        [Description("Некорректная разрядность итога чека")]
        WrongSumDepth = 0x3B,

        [Description("Некорректная разрядность денежных величин")]
        WrongMoneyDepth = 0x3C,

        [Description("Превышено максимально допустимое количество предметов расчета в чеке")]
        WrongItemsCount = 0x3D,

        [Description("Превышено максимально допустимое количество предметов расчета c данными агента в чеке")]
        WrongAgentItemsCount = 0x3E,

        [Description("Невозможно передать данные агента, допустимы данные агента либо для всего чека, либо данные агента по предметам расчета")]
        WrongAgentData = 0x3F,

        [Description("Некорректный статус печатающего устройства")]
        WrongStatePrinter = 0x40,

        [Description("Сумма изъятия больше доступной суммы наличных в ККТ")]
        WrongWithdrawalAmount = 0x42,

        [Description("Операция внесения-изъятия денег в ККТ возможна только при открытой смене")]
        DepositWithdrawalIsOpenedShiftOnly = 0x43,

        [Description("Счетчики денег не инициализированы")]
        MoneyCountersNotInitialized = 0x44,

        [Description("Сумма по чеку коррекции всеми типами оплаты не равна полной сумме для расчетов по ставкам НДС")]
        IncorrectCorrectionVat = 0x45,

        [Description("Сумма по чеку коррекции всеми типами оплаты не равна итоговой сумме чека коррекции")]
        IncorrectCorrectionSum = 0x46,

        [Description("В чеке коррекции не указано ни одной суммы для расчетов по ставкам НДС")]
        IncorrectCorrectionSumForVat = 0x47,

        [Description("Ошибка сохранения настроек")]
        SavingSettingsError = 0x50,

        [Description("Передано некорректное значение времени")]
        WrongTimeValue = 0x51,

        [Description("В чеке не должны присутствовать иные предметы расчета помимо предмета расчета с признаком способа расчета Оплата кредита")]
        OtherCalculationSubject = 0x52,

        [Description("Переданы не все необходимые данные для агента")]
        FewDataForAgent = 0x53,

        [Description("Итоговая сумма чека не равна сумме оплаты всеми видами")]
        WrongSum2 = 0x54,

        [Description("Неверно указан признак расчета для чека коррекции, возможные значения: приход, расход")]
        WrongCalculationSignCorrection = 0x55,

        [Description("Неверная структура переданных данных для агента")]
        WrongDataForAgent = 0x56,

        [Description("Не указан режим налогообложения")]
        NoTaxMode = 0x57,

        [Description("Данная ставка НДС недопустима для агента. Агент не является плательщиком НДС")]
        WrongTaxForAgent = 0x58,

        [Description("Некорректно указано значение тэга Признак платежного агента")]
        WrongTaxValueSign = 0x59,

        [Description("Невозможно внести товарную позицию после внесения данных об оплате")]
        InsertItemError = 0x5A,

        [Description("Команда может быть выполнена только при открытом чеке")]
        DocumentIsClosed = 0x5B,

        [Description("Некорректный формат или длина в массиве переданных строк нефискальной информации")]
        IncorrectNonFiscalDataFormat = 0x5C,

        [Description("Достигнуто максимальное количество строк нефискальной информации")]
        NonFiscalDataTooLong = 0x5D,

        [Description("Не переданы данные кассира")]
        CashierInfoNotPresented = 0x5E,

        [Description("Невозможно передать параметры автоматического устрйоства в кассовый чек для ККТ, уже зарегистрированной с этими параметрами")]
        UnableSendAutomaticDeviceData = 0x5F,

        [Description("Номер блока прошивки указан некорректно")]
        WrongBlockFirmware = 0x60,

        [Description(" Значение не зашито в ККТ")]
        ValueMissingInFirmware = 0x70,

        [Description("Некорректное значение серийного номера")]
        IncorrectSerialNumber = 0x71,

        [Description("Команда не выполнена")]
        CommandNotExecute = 0x7F,

        [Description("Присутствуют неотправленные в ОФД документы")]
        NotSendedDocuments = 0xE0,

        [Description("Подключенный ФН не соответствует данным регистрации ККТ")]
        WrongRegistrationData = 0xF3,

        [Description("ФН еще не был активирован")]
        FnNotRegistred = 0xF4,

        [Description("ФН был закрыт")]
        FnIsClosed = 0xF5,

        [Description("Передать данные автоматического устройства расчетов для кассового чека (БСО)")]
        NeedMashineDataForBso = 0x1F,

        [Description("Передать данные автоматического устройства расчетов для кассового чека (БСО) коррекции")]
        NeedMashineDataForBsoCorrection = 0x3F,

        [Description("Неизвестная ошибка")]
        UnknownError = 0xFF,

        [Description("Неизвестное исключение")]
        UnknownErrorExc = 0xFE,

        [Description("Нет связи с ККТ")]
        ConnectError = 0xFD
    }

    //==============================================================================================================================================

    public enum TerminalFAFnConnected
    {
        OK = 1,
        NoDevice = 0
    }
}