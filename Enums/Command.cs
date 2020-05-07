namespace KKT_APP_FA.Enums
{
    public enum CommandEnum
    {
        GET_STATUS = 0x01, // Запрос статуса ККТ (0x01)
        GET_SERIAL_NUMBER = 0x02, // Запрос заводского номера ККТ(0x02)
        GET_FIRMWARE_VERSION = 0x03, // Запрос версии ПО ККТ 
        GET_MODEL = 0x04, // Запрос модели ККТ 0x04
        GET_FN_NUMBER = 0x05, // Получить номер ФН
        GET_FN_FIRMWARE_VERSION = 0x06, // Запрос версии ПО ФН (0x06)
        GET_FN_EXPIRED = 0x07, // Запрос срока действия ФН (0x07)
        GET_FISCAL_STORAGE_STATUS = 0x08, // 
        GET_REGISTRATION_PARAMETERS = 0x0A, // Запрос текущих параметров регистрации ККТ
        GET_KKT_CONFIGURATION_VERSION = 0x0B, // Запрос версии конфигурации ККТ (0x0B)
        GET_TCP_IP_PARAMS = 0x0E, // Запрос текущих параметров TCP/IP (0x0E)
        CANCEL_FISCAL_DOCUMENT = 0x10, // Отменяет любой начатый фискальный документ. Все данные, введенные с помощью команды "Передать данные документа" удаляются
        SEND_AUTOMATIC_DEVICE_DATA = 0x1F, // Передать данные автоматического устройства расчетов для кассового чека (0x1F)
        GET_SHIFT_INFO = 0x20, // Запрос параметров текущей смены (0x20)
        BEGIN_OPEN_SESSION = 0x21, // Начать открытие смены (0x21)
        OPEN_SESSION = 0x22, //Открыть смену (0x22)
        OPEN_CHECK = 0x23, // Открыть кассовый чек (0x23)
        CHECK = 0x24, // Сформировать чек (0x24)
        OPEN_CORRECTION_CHECK = 0x25, // Открыть кассовый чек коррекции (0x25)
        CORRECTION_CHECK = 0x26, // Сформировать чек коррекции (0x26)       
        BEGIN_CLOSE_SESSION = 0x29, // Начать закрытие смены (0x29)
        CLOSE_SESSION = 0x2A, // Закрыть смену (0x2A). Условия: должны быть выполнены команды 0x29 и 0x2F (если не в автоматическом режиме)
        //BEGIN_CHECK = 0x23,
        CHECK_POSITION = 0x2B, // Передать данные предмета расчета (0x2B)
        AGENT_DATA = 0x2C,
        PAYMENT_DATA = 0x2D, // Передать Данные оплаты (0x2D)
        CORRECTION_DATA = 0x2E, // Передать данные чека коррекции (0x2E)
        CASHIER_DATA = 0x2F, // Передать данные кассира(0x2F). Для ККТ в составе автоматических устройств самообслуживания - необязательно
        SEND_CORRECTION_AUTOMATIC_DEVICE_DATA = 0x3F, // Передать данные автоматического устройства расчетов для кассового чека (БСО) коррекции (0x3F)
        PRINT = 0x61,
        CUT = 0x62,
        REGISTRATION_PARAMETERS = 0x0A,
        CANCEL_DOCUMENT = 0x10,
        GET_OFD_INFORMATION_EXCHANGE_STATUS = 0x50, // Запрос статуса информационного обмена с ОФД (0x50) 

        KKT_RESTART = 0xEF // Перезагрузка ККТ
    }
}