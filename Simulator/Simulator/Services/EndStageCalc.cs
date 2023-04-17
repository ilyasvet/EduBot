using Simulator.BotControl;
using Simulator.Models;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.Services
{
    internal static class EndStageCalc
    {
        public static ValueTuple<string, InlineKeyboardMarkup> GetResultOfModule(CaseStageEndModule descriptor, long userId)
        {
            double currentRate = 0;
            List<InlineKeyboardButton[]> buttons = new List<InlineKeyboardButton[]>()
            {
                new[]{ CommandKeyboard.ToFinishButton },
            };
            currentRate = UserCaseTableCommand.GetRate(userId);
            int attemptNumber = UserCaseTableCommand.FirstAttemptIsNull(userId) ? 1 : 2;
            if(descriptor.IsEndOfCase)
            {
                if(currentRate < descriptor.Rates[0])
                {
                    int ratePlace = 0;
                    return GetResult(attemptNumber, descriptor, buttons, ratePlace); 
                }
                else if(currentRate < descriptor.Rates[1])
                {
                    int ratePlace = 1;
                    return GetResult(attemptNumber, descriptor, buttons, ratePlace);
                }
                else
                {
                    int ratePlace = 1;
                    return GetResult(attemptNumber, descriptor, buttons, ratePlace);
                }
            }
            if(currentRate < descriptor.Rates[0])
            {
                
            }
        }
        private static ValueTuple<string, InlineKeyboardMarkup> GetResult(
            int attemptNumber,
            CaseStageEndModule descriptor,
            List<InlineKeyboardButton[]> buttons,
            int ratePlace)
        {
            var result = new ValueTuple<string, InlineKeyboardMarkup>();
            if (attemptNumber == 1)
            {
                result.Item1 = descriptor.Texts[ratePlace];
                buttons.Add(new[] { CommandKeyboard.ToBeginButton });
                result.Item2 = new InlineKeyboardMarkup(buttons);
            }
            else
            {
                result.Item1 = descriptor.Texts[ratePlace+1];
            }
            return result;
        }
    }
}
