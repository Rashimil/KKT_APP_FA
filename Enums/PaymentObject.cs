using System.ComponentModel;

namespace KKT_APP_FA.Enums
{
    // Признаки предмета расчета (Тэг 1212):
    public enum PaymentObjectEnum : byte
    {
        [Description("Товар, за исключением подакцизного товара")]
        Commodity = 1,

        [Description("Подакцизный товар")]
        Excise = 2,

        [Description("Работа")]
        Job = 3,

        [Description("Услуга")]
        Service = 4,

        [Description("Приеме ставок по проведению азартных игр")]
        GamblingBet = 5,

        [Description("Выплата выигрыша по проведению азартных игр")]
        GamblingPrize = 6,

        [Description("Лотерея")] // отсутствует в руководстве ТерминалФА
        Lottery = 7,

        [Description("Выплата выигрыша по проведению лотерей")]
        LotteryPrize = 8,

        [Description("Предоставление прав на использование результатов интеллектуальной деятельности или средств индивидуализации")]
        IntellectualActivity = 9,

        [Description("Авансе, задаток, предоплата, кредит, взнос в счет оплаты, пени, штраф, вознагражденеи, бонус, иной аналогичный предмет расчета")]
        Payment = 10,

        [Description("Агентское вознаграждение")]
        AgentCommission = 11,

        [Description("Предмет расчета, состоящий из предметов, каждому из которых может быть присвоено значение от 1 до 11")]
        Composite = 12,

        [Description("Предмет расчета, НЕ относящийся к предметам расчета, которым может быть присвоено значение от 1 до 12 и от 14 до 18")]
        Another = 13,

        [Description("Передача имущественных прав")]
        PropertyRights = 14,

        [Description("Внереализационный доход")]
        NoOperatingIncome = 15,

        [Description("Сумма расходов, уменьшающих сумму налога (авансовых платежей)")]
        TaxReduction = 16,

        [Description("Сумма уплаченного торгового сбора")]
        TradeFee = 17,

        [Description("Курортный сбор")]
        ResortFee = 18,

        [Description("Залог")]
        Pledge = 19,

    }
}
