using KKT_APP_FA.Enums;
using KKT_APP_FA.Helpers;
using KKT_APP_FA.Models;
using KKT_APP_FA.Models.API;
using KKT_APP_FA.Models.KKTRequest;
using KKT_APP_FA.Models.KKTResponse;
using KKT_APP_FA.StaticValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKT_APP_FA.Units
{
    // Основной модуль с которым работает главный цикл ( абстракция: ККТHigh => ККТ => LogicLevel => ТерминалФА ):
    public class KKTHighLevel
    {
        KKT kkt;
        //=======================================================================================================================================
        public KKTHighLevel()
        {
            kkt = new KKT();
        }

        //=======================================================================================================================================

        // РЕГИСТРАЦИЯ ЧЕКА
        public KKTHighLevelResponse Register(Registration body, string operation)
        {
            KKTHighLevelResponse response = new KKTHighLevelResponse();

            // Получение данных кассира:
            CashierData cashierData = null;
            if (!Program.IsAutomaticDevice)
            {
                cashierData = new CashierData(body.receipt.company.payment_address, body.receipt.company.inn);
            }
            
            // Получение данных клиента:
            ClientData clientData = new ClientData(body.receipt.client.email, "");

            // Получение taxType (СНО):
            TaxTypeEnum taxType;
            switch (body.receipt.company.sno.ToLower()) // СНО пользователя. 
            {
                case "osn":
                    taxType = TaxTypeEnum.Common;
                    break;
                case "usn_income":
                    taxType = TaxTypeEnum.Simplified;
                    break;
                case "usn_income_outcome":
                    taxType = TaxTypeEnum.Simplified2;
                    break;
                case "envd":
                    taxType = TaxTypeEnum.ENVD;
                    break;
                case "esn":
                    taxType = TaxTypeEnum.ESN;
                    break;
                case "patent":
                    taxType = TaxTypeEnum.Patent;
                    break;
                default:
                    taxType = TaxTypeEnum.Common;
                    break;
            }

            // Получение типов оплаты:
            decimal CASH = 0;
            decimal ELECTRONICALLY = 0;
            decimal PREPAID = 0;
            decimal CREDIT = 0;
            decimal OTHER = 0;
            foreach (var item in body.receipt.payments)
            {
                switch (item.type)
                {
                    case 1:
                        ELECTRONICALLY += (decimal)item.sum; // LIBFPTR_PT_ELECTRONICALLY - электронный
                        break;
                    case 2:
                        PREPAID += (decimal)item.sum; //  предварительная оплата(аванс)
                        break;
                    case 3:
                        CREDIT += (decimal)item.sum; //  последующая оплата(кредит)
                        break;
                    case 4:
                        OTHER += (decimal)item.sum; //  иная форма оплаты(встречное предоставление)
                        break;
                    case 5:
                        CASH += (decimal)item.sum; // иная форма оплаты(встречное предоставление)
                        break;
                    default:
                        CASH += (decimal)item.sum; // По умолчанию  CASH - наличные
                        break;
                }
            }

            // Получение AutomaticDeviceData:
            AutomaticDeviceData automaticDeviceData = null;
            if (Program.IsAutomaticDevice)
            {
                automaticDeviceData = new AutomaticDeviceData(
                    body.receipt.company.payment_address,
                    "Центр обработки данных",
                    " " // ХЗ ГДЕ БРАТЬ НОМЕР АВТОМАТА !!!!!!! Этого не было в протоколе атола, надо добавлять!!!!
                    );
            }

            // Получение RegisterCheck:
            OperationEnum Operation;
            switch (operation.ToLower())
            {
                case "sell":
                    Operation = OperationEnum.Sell; // LIBFPTR_RT_SELL - чек прихода (продажи)
                    break;
                case "sell_refund":
                    Operation = OperationEnum.SellRefund; // LIBFPTR_RT_SELL_RETURN - чек возврата прихода(продажи)
                    break;
                case "buy":
                    Operation = OperationEnum.Buy; // LIBFPTR_RT_BUY - чек расхода(покупки)
                    break;
                case "buy_refund":
                    Operation = OperationEnum.BuyRefund; // LIBFPTR_RT_BUY_RETURN - чек возврата расхода(покупки)
                    break;
                default:
                    Operation = OperationEnum.Sell; // По умолчанию LIBFPTR_RT_SELL - чек прихода (продажи)
                    break;
            }
            RegisterCheck registerCheck = new RegisterCheck(
                Operation,
                (decimal)body.receipt.total,
                ""
                );

            //-----------------------------------------------------------------------------------------------------------------------------------
            // На всякий случай - отменить открытый фискальный документ
            kkt.CancelFiscalDocument();

            //-----------------------------------------------------------------------------------------------------------------------------------
            // Удостовериться что
            //       а) смена открыта (GetShiftInfo)
            response.GetShiftInfo = kkt.GetShiftInfo();
            if (!response.GetShiftInfo.ShiftOpened) // смена закрыта, нужно закрыть и открыть заново:
            {
                var oShift = OpenShift(body);
                response.OpenShift = oShift.OpenShiftResponse; 
                if (response.OpenShift != null)
                {
                    // Смена успешно открыта
                    response.GetShiftInfo.ShiftOpened = true;
                    response.error = false;
                    response.GetShiftInfo.Result = 0;
                }
                else
                {
                    // Смену открыть не удалось
                    response.GetShiftInfo.ShiftOpened = false;
                    response.error = true;
                    response.error_code = oShift.BaseResponse.Result;
                    response.error_text = oShift.BaseResponse.Description;
                    response.error_type = oShift.BaseResponse.ErrorCode.ToString();
                }
            }

            if (response.GetShiftInfo.Result == 0)
            {
                // Смена открыта, идем далее
                //-----------------------------------------------------------------------------------------------------------------------------------
                // Удостовериться что
                //       б) что не прошло 24 часа (пытаться OpenCheck, если ошибка 22 - то закрыть и открыть смену)
                response.OpenCheck = kkt.OpenCheck();
                if (response.OpenCheck.Result == 22) //  0x16 - смена истекла 
                {
                    var cShift = CloseShift(body); // закрыть смену
                    response.CloseShift = cShift.CloseShiftResponse;
                    if (response.CloseShift == null)
                    {
                        // TODO подумать как переоткрывать смену или лочить ККТ
                    }     
                    var oShift = OpenShift(); // открыть смену
                    response.OpenShift = oShift.OpenShiftResponse;
                    if (response.OpenShift == null)
                    {
                        // TODO подумать как переоткрывать смену или лочить ККТ
                    }
                    response.OpenCheck = kkt.OpenCheck();
                }

                if (response.OpenCheck.Result == 0)
                {
                    // Чек успешно открыт, идем далее
                    //-----------------------------------------------------------------------------------------------------------------------------------
                    // В цикле выполнять SendCheckPosition
                    foreach (var item in body.receipt.items)
                    {
                        // VAT конвертер:
                        VatEnum vat;
                        try
                        {
                            switch (item.vat.type.ToLower())
                            {
                                case "vat18":
                                    vat = VatEnum.Vat20;
                                    break;
                                case "vat10":
                                    vat = VatEnum.Vat10;
                                    break;
                                case "vat118":
                                    vat = VatEnum.Vat20120;
                                    break;
                                case "vat110":
                                    vat = VatEnum.Vat10110;
                                    break;
                                case "vat0":
                                    vat = VatEnum.Vat0;
                                    break;
                                case "none":
                                    vat = VatEnum.None;
                                    break;
                                case "vat20":
                                    vat = VatEnum.Vat20;
                                    break;
                                case "vat120":
                                    vat = VatEnum.Vat20120;
                                    break;
                                default:
                                    vat = VatEnum.None;
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            vat = VatEnum.None;
                        }

                        // PaymentMethod конвертер:
                        PaymentMethodEnum paymentMethod;
                        try
                        {
                            switch (item.payment_method.ToLower())
                            {
                                case "full_prepayment":
                                    paymentMethod = PaymentMethodEnum.FullPrepayment; // 1 - Предоплата 100 %
                                    break;
                                case "prepayment":
                                    paymentMethod = PaymentMethodEnum.Prepayment; // 2 - Предоплата
                                    break;
                                case "advance":
                                    paymentMethod = PaymentMethodEnum.Advance; // 3 - Аванс
                                    break;
                                case "full_payment":
                                    paymentMethod = PaymentMethodEnum.FullPayment; // 4 - Полный расчет
                                    break;
                                case "partial_payment":
                                    paymentMethod = PaymentMethodEnum.PartialPayment; // 5 - Частичный расчет и кредит
                                    break;
                                case "credit":
                                    paymentMethod = PaymentMethodEnum.Credit; // 6 - Передача в кредит
                                    break;
                                case "credit_payment":
                                    paymentMethod = PaymentMethodEnum.CreditPayment;  // 7 - Оплата кредита
                                    break;
                                default:
                                    paymentMethod = PaymentMethodEnum.FullPrepayment; // По умолчанию 1 - Предоплата 100 % (совпадает с API)
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            paymentMethod = PaymentMethodEnum.FullPrepayment; // По умолчанию 1 - Предоплата 100 % (совпадает с API)
                        }

                        // PaymentObject конвертер:
                        PaymentObjectEnum paymentObject;
                        try
                        {
                            switch (item.payment_object.ToLower())
                            {
                                case "commodity":
                                    paymentObject = PaymentObjectEnum.Commodity; // 1 - Товар
                                    break;
                                case "excise":
                                    paymentObject = PaymentObjectEnum.Excise; // 2 - Подакцизный товар
                                    break;
                                case "job":
                                    paymentObject = PaymentObjectEnum.Job; // 3 - Работа
                                    break;
                                case "service":
                                    paymentObject = PaymentObjectEnum.Service; // 4 - Услуга
                                    break;
                                case "gambling_bet":
                                    paymentObject = PaymentObjectEnum.GamblingBet; // 5 - Ставка азартной игры
                                    break;
                                case "gambling_prize":
                                    paymentObject = PaymentObjectEnum.GamblingPrize; // 6 - Выигрыш азартной игры
                                    break;
                                case "lottery":
                                    paymentObject = PaymentObjectEnum.Lottery; // 7 - Лотерейный билет
                                    break;
                                case "lottery_prize":
                                    paymentObject = PaymentObjectEnum.LotteryPrize; // 8 - Выигрыш лотереи
                                    break;
                                case "intellectual_activity":
                                    paymentObject = PaymentObjectEnum.IntellectualActivity; // 9 - Предоставление РИД
                                    break;
                                case "payment":
                                    paymentObject = PaymentObjectEnum.Payment; // 10 - Платеж
                                    break;
                                case "agent_commission":
                                    paymentObject = PaymentObjectEnum.AgentCommission; // 11 - Агентское вознаграждение
                                    break;
                                case "composite":
                                    paymentObject = PaymentObjectEnum.Composite; // 12 - Составной предмет расчета
                                    break;
                                case "another":
                                    paymentObject = PaymentObjectEnum.Another; // 13 - Иной предмет расчета
                                    break;
                                default:
                                    paymentObject = PaymentObjectEnum.Commodity; // По умолчанию 1 - Товар (совпадает с API)
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            paymentObject = PaymentObjectEnum.Commodity; // По умолчанию 1 - Товар (совпадает с API)
                        }

                        // Составление CheckItem:
                        CheckItem checkItem = new CheckItem(
                            item.name,                  // наименование
                            (decimal)item.price,        // цена
                            item.quantity,              // количество
                            vat,                        // ставка НДС
                            paymentMethod,              // признак способа расчета (Тэг 1214)
                            paymentObject,              // признак предмета расчета (Тэг 1212):
                            "",                         // код довара (КТН)
                            item.measurement_unit,      // единица измерения
                            0,                          // акциз
                            "",                         // номер таможенной декларации
                            "",                         // код страны
                            ""                          // доп. реквизит
                            );
                        var br = kkt.SendCheckPosition(checkItem);
                        response.SendCheckPosition.Add(br);
                        if (br.Result == 0)
                        {
                            // ошибок нет
                            response.error = false;
                        }
                        else
                        {
                            // ошибка при добавлении позиции чека
                            response.error = true;
                            response.error_code = br.Result;
                            response.error_text = br.Description;
                            response.error_type = br.ErrorCode.ToString();
                            break; // да, да...
                        }
                    }
                    if (!response.error)
                    {
                        // Нет ошибок при регистрации позиций чека, можно двигаться дальше
                        //-----------------------------------------------------------------------------------------------------------------------------------
                        //   6. SendPaymentData
                        PaymentData paymentData = new PaymentData(
                        taxType,            // Тип налогообложения
                        CASH,               // итого наличными
                        ELECTRONICALLY,     // итого электронными
                        PREPAID,            // итого предоплатой
                        CREDIT,             // итого в кредит
                        OTHER,              // итого прочее
                        cashierData,        // данные кассира
                        clientData          // данные покупателя
                        );
                        response.SendPaymentData = kkt.SendPaymentData(paymentData);
                        if (response.SendPaymentData.Result == 0)
                        {
                            // нет ошибок при SendPaymentData, можно двигаться дальше
                            //-----------------------------------------------------------------------------------------------------------------------------------
                            //   7. SendAutomaticDeviceData (если автомат)
                            response.SendAutomaticDeviceData = kkt.SendAutomaticDeviceData(automaticDeviceData);
                            if (response.SendAutomaticDeviceData.Result == 0)
                            {
                                // нет ошибок при SendAutomaticDeviceData, можно двигаться дальше
                                //-----------------------------------------------------------------------------------------------------------------------------------
                                //   8. RegisterCheck
                                response.RegisterCheck = kkt.RegisterCheck(registerCheck);
                                if (response.RegisterCheck.Result == 0)
                                {
                                    // Всё прошло успешно, можно выдавать инфу чеке:
                                    response.total = CASH + ELECTRONICALLY + PREPAID + CREDIT + OTHER;
                                    response.fns_site = "nalog.ru";
                                    response.fn_number = KktStaticValues.FN;
                                    response.shift_number = response.RegisterCheck.ShiftNumber;
                                    response.receipt_datetime = response.RegisterCheck.CheckDateTime;
                                    response.fiscal_receipt_number = response.RegisterCheck.CheckNumber;
                                    response.fiscal_document_number = Convert.ToInt32(response.RegisterCheck.FD);
                                    response.ecr_registration_number = KktStaticValues.KKTRegistrationNumber;
                                    response.fiscal_document_attribute = Convert.ToInt64(response.RegisterCheck.FPD);
                                    response.daemon_code = "KKT-APP-" + KktStaticValues.KKTFactoryNumber;
                                    response.device_code = KktStaticValues.FirmwareVersion;
                                    response.error_code = 0;
                                    response.error_text = null;
                                    response.error_type = null;
                                }
                                else
                                {
                                    // есть ошибки при RegisterCheck
                                    response.error = true;
                                    response.error_code = response.RegisterCheck.Result;
                                    response.error_text = response.RegisterCheck.Description;
                                    response.error_type = response.RegisterCheck.ErrorCode.ToString();
                                }
                            }
                            else
                            {
                                // есть ошибки при SendAutomaticDeviceData
                                response.error = true;
                                response.error_code = response.SendAutomaticDeviceData.Result;
                                response.error_text = response.SendAutomaticDeviceData.Description;
                                response.error_type = response.SendAutomaticDeviceData.ErrorCode.ToString();
                            }
                        }
                        else
                        {
                            // были ошибки при SendPaymentData
                            response.error = true;
                            response.error_code = response.SendPaymentData.Result;
                            response.error_text = response.SendPaymentData.Description;
                            response.error_type = response.SendPaymentData.ErrorCode.ToString();
                        }
                    }
                    else
                    {
                        // Были ошибки при добавлении позиций чека 
                        response.error = true;
                        // остальные поля (error_code, error_text, error_type) уже заполнены в цикле
                    }
                }
                else
                {
                    // Чек открыт не удалось
                    response.error = true;
                    response.error_code = response.OpenCheck.Result;
                    response.error_text = response.OpenCheck.Description;
                    response.error_type = response.OpenCheck.ErrorCode.ToString();
                }
            }
            else
            {
                // Смену открыть не удалось
                response.GetShiftInfo.ShiftOpened = false;
                response.error = true;
                // остальные поля (error_code, error_text, error_type) уже заполнены выше
            }
            if (response.error) response.error_type = "driver"; // для совместимости с логикой АТОЛ
            return response;
        }

        //=======================================================================================================================================

        // КОРРЕКЦИИ
        public dynamic Correction(Correction body, string operation, bool no_print_receipt = true)
        {
            return null;
        }

        //=======================================================================================================================================

        // ОТКРЫТИЕ СМЕНЫ
        public (BaseResponse BaseResponse, OpenShiftResponse OpenShiftResponse) OpenShift(Registration body = null)
        {
            OpenShiftResponse osr;
            BaseResponse br;

            br = kkt.OpenShiftBegin();
            if (br.Result != 0)
            {
                return (br, null);
            }           
            if (body != null && !Program.IsAutomaticDevice) // Отправляем данные кассира, если они есть и ККТ не для автомата:
            {
                br = kkt.SendCashierData(new CashierData(body.receipt.company.payment_address, body.receipt.company.inn));
                if (br.Result != 0)
                {
                    // return (br, null); НЕ возвращаем ошибку, т. к. отсутствие этого особо ни на что не влияет 
                }
            }
            osr = kkt.OpenShift();
            return (null, osr);
        }

        //=======================================================================================================================================

        // ЗАКРЫТИЕ СМЕНЫ
        public (BaseResponse BaseResponse, CloseShiftResponse CloseShiftResponse) CloseShift(Registration body = null)
        {
            CloseShiftResponse csr;
            BaseResponse br;

            br = kkt.CloseShiftBegin();
            if (br.Result != 0)
            {
                return (br, null);
            }
            if (body != null && !Program.IsAutomaticDevice) // Отправляем данные кассира, если они есть и ККТ не для автомата:
            {
                br = kkt.SendCashierData(new CashierData(body.receipt.company.payment_address, body.receipt.company.inn));
                if (br.Result != 0)
                {
                    // return (br, null); НЕ возвращаем ошибку, т. к. отсутствие этого особо ни на что не влияет 
                }
            }
            csr = kkt.CloseShift();
            return (null, csr);
        }

        //=======================================================================================================================================

        // ПРОВЕРКА СОСТОЯНИЯ СМЕНЫ (открыта или нет)
        public bool? ShiftIsOpen()
        {
            // TODO сделать метод ShiftStateReset, инкапсулировав в него всё что возможно для корректного возвращения ККТ из любого состояния в состояние готовности принимать чек
            return null;
        }

        //=======================================================================================================================================

        // Получение статичных полей респонсов из ККТ (номер ФН, серийный номер, рег. номер и т. д.)
        public void GetStaticResponseFields()
        {
            KktStaticValues.FN = kkt.GetFnNumber().FN;
            KktStaticValues.KKTFactoryNumber = kkt.GetKktStatus().KKTFactoryNumber;
            GetRegistrationParametersResponse registrationParameters = kkt.GetRegistrationParameters();
            KktStaticValues.KKTRegistrationNumber = registrationParameters.KKTRegistrationNumber;
            KktStaticValues.INN = registrationParameters.INN;
            KktStaticValues.KKTOperatingMode = registrationParameters.KKTOperatingMode;
            KktStaticValues.TaxTypes = registrationParameters.TaxTypes;
            KktStaticValues.AgentType = registrationParameters.AgentType;
            KktStaticValues.FirmwareVersion = kkt.GetFirmwareVersion().FirmwareVersion;
        }

        //=======================================================================================================================================

        // Получение информации от ККТ. Занимает время 
        public object GetKktInfo()
        {
            return null;
        }

        //=======================================================================================================================================
        //=======================================================================================================================================
    }
}
