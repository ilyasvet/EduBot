using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.Case
{
    internal static class StagesControl
    {
        public static StageList Stages { get; set; }
        
        public static double CalculateRatePoll(CaseStagePoll stage, int[] answers)
        {
            double rate = 0;
            foreach (var answer in answers)
            {
                rate += stage.PossibleRate[answer];
            }
            return rate;
        }
        public static void SetStageForMove(CaseStagePoll stage, int[] answers)
        {
            if(stage.ConditionalMove)
            {
                stage.NextStage = stage.MovingNumbers[answers[0]];
            } 
            //Если переход безусловный, то NextStage уже установлено
            //Если нет, то на основе свойств и ответа выбираем NextStage
        }
        public static async Task AlertNextButton(long userId, CaseStage thisStage, ITelegramBotClient botClient)
        {
            //Всегда, кроме последнего вопроса

            List<InlineKeyboardButton[]> inlineKeyboardCallBack = new List<InlineKeyboardButton[]>();
            if (thisStage.IsEndOfModule)
            {
                InlineKeyboardButton ToBeginButton = InlineKeyboardButton.WithCallbackData("В начало модуля", "ToBegin");
                inlineKeyboardCallBack.Add(new[] { ToBeginButton });
            }
            if (thisStage.IsEndOfCase)
            {
                InlineKeyboardButton ToFinishButton = InlineKeyboardButton.WithCallbackData("К завершению", "ToEnd");
                inlineKeyboardCallBack.Add(new[] { ToFinishButton });
            }
            else
            {
                InlineKeyboardButton NextButton = InlineKeyboardButton.WithCallbackData("Далее", "MoveNext");
                inlineKeyboardCallBack.Add(new[] { NextButton });
            }
            await botClient.SendTextMessageAsync(
                chatId: userId,
                text:thisStage.TextAfter,
                replyMarkup: new InlineKeyboardMarkup(inlineKeyboardCallBack));
            //Если конец модуля, есть возможность вернуться в его начало и пройти заново
            //Если конец кейса, то переход к заключению
            //Если не конец, то переход к следующему этапу
        }
        public static CaseStage GetNextStage(CaseStage current, CallbackQuery query)
        {
            if (query.Data == "MoveNext")
            {
                return Stages[current.NextStage];
            }
            else if(query.Data == "ToBegin")
            {
                return Stages.Stages.Where(s => s.ModuleNumber == current.ModuleNumber).Min();
            }
            else if(query.Data == "ToEnd")
            {
                return Stages[-1]; //-1 - номер этапа заключения
            }
            throw new ArgumentException();
            //Если пользователь нажал далее, то переходим через свойство NextStage, которое либо было изначально
            //установлено, либо установлено вследствие условного перехода.
            //Если пользователь нажал вернуться в начало модуля, то возвращаем первый этап текущего модуля 
            //TODO придумать, что делать с его очками во 2 случае
        }
        public static async Task MoveNext(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            switch (nextStage)
            {
                case CaseStagePoll poll:
                    await botClient.SendPollAsync(
                        chatId: userId,
                        question: poll.TextBefore,
                        options: poll.Options,
                        isAnonymous: false,
                        allowsMultipleAnswers: poll.ManyAnswers);
                    break;
                default:
                    break;
            }
            //В зависимости от типа этапа, бот выдаёт определённый тип сообщения
        }
    }
}
