using Simulator.BotControl;
using Simulator.Models;
using Simulator.TelegramBotLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.Services
{
    enum ResultType
    {
        FirstFail,
        SecondFail,
        Success,
    }

    internal static class EndStageCalc
    {
        public async static Task<ValueTuple<string, InlineKeyboardMarkup>> GetResultOfModule(CaseStageEndModule descriptor, long userId)
        {
            double currentRate;
            List<InlineKeyboardButton[]> buttons = new List<InlineKeyboardButton[]>();
            var result = new ValueTuple<string, InlineKeyboardMarkup>();
            
            currentRate = await DataBaseControl.UserCaseTableCommand.GetRate(userId);
            int attemptNumber = await DataBaseControl.UserCaseTableCommand.GetHealthPoints(userId) > 1 ? 1 : 2;
            //Если жизней больше, чем 1, то это первая попытка, иначе 2.
            
            if(descriptor.IsEndOfCase)
            {
                int ratePlace;
                if (currentRate < descriptor.Rates[0])
                {
                    ratePlace = 0;
                }
                else if(currentRate < descriptor.Rates[1])
                {
                    ratePlace = 1;
                }
                else
                {
                    ratePlace = 2;
                }
                result = GetResultEnd(attemptNumber, descriptor, buttons, ratePlace);
                await DataBaseControl.UserCaseTableCommand.SetHealthPoints(userId, 2-attemptNumber);
                //Если была первая попытка, то поставится 1, если вторая, то 0
            }
            else if(currentRate < descriptor.Rates[0])
            {
                int newHealthPoints = 0;
                if(descriptor.ModuleNumber == 1 &&
                    await DataBaseControl.UserCaseTableCommand.GetHealthPoints(userId) == 3)
                {
                    newHealthPoints = 2;
                    //Снимается дополнительная попытка на 1 модуль

                    result = GetResult(buttons, descriptor, ResultType.FirstFail);
                }
                else if(attemptNumber == 1)
                {
                    newHealthPoints = 1;
                    result = GetResult(buttons, descriptor, ResultType.FirstFail);
                }
                else if(attemptNumber == 2)
                {
                    newHealthPoints = 0;
                    result = GetResult(buttons, descriptor, ResultType.SecondFail);
                }
                await DataBaseControl.UserCaseTableCommand.SetHealthPoints(userId, newHealthPoints);
            }
            else
            {
                result = GetResult(buttons, descriptor, ResultType.Success);
            }
            return result;
        }
        private static ValueTuple<string, InlineKeyboardMarkup> GetResultEnd(
            int attemptNumber,
            CaseStageEndModule descriptor,
            List<InlineKeyboardButton[]> buttons,
            int ratePlace)
        {
            var result = new ValueTuple<string, InlineKeyboardMarkup>();
            result.Item1 = descriptor.Texts[ratePlace] + "\n"; //Информация о результате
            if (attemptNumber == 1)
            {
                result.Item1 += descriptor.Texts[4]; //Сказать, что есть ещё попытка
                buttons.Add(new[] { CommandKeyboard.ToBeginButton });
            }
            else
            {
                result.Item1 += descriptor.Texts[3]; //Сказать, что попыток больше нет
                buttons.Add(new[] { CommandKeyboard.ToFinishButton });
            }
            result.Item2 = new InlineKeyboardMarkup(buttons);
            return result;
        }
        private static ValueTuple<string, InlineKeyboardMarkup> GetResult(
            List<InlineKeyboardButton[]> buttons,
            CaseStageEndModule descriptor,
            ResultType resultType
            )
        {
            var result = new ValueTuple<string, InlineKeyboardMarkup>();
            switch (resultType)
            {
                case ResultType.FirstFail:
                    result.Item1 = descriptor.Texts[0] + "\n" + descriptor.Texts[3];
                    buttons.Add(new[] { CommandKeyboard.ToBeginButton });
                    break;
                case ResultType.SecondFail:
                    result.Item1 = descriptor.Texts[0] + "\n" + descriptor.Texts[2];
                    buttons.Add(new[] { CommandKeyboard.NextButton });
                    break;
                case ResultType.Success:
                    result.Item1 = descriptor.Texts[1];
                    buttons.Add(new[] { CommandKeyboard.NextButton });
                    break;
                default:
                    break;
            }
            result.Item2 = new InlineKeyboardMarkup(buttons);
            return result;

        }
    }
}
