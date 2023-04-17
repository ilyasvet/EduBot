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

        private static InlineKeyboardButton ToFinishButton = InlineKeyboardButton.WithCallbackData("Выйти", "ToOut");
        private static InlineKeyboardButton NextButton = InlineKeyboardButton.WithCallbackData("Далее", "MoveNext");
        private static InlineKeyboardMarkup StageMenu = new(new List<InlineKeyboardButton[]>
        {
            new[] { ToFinishButton },
            new[] { NextButton }
        });

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
            await botClient.SendTextMessageAsync(
                chatId: userId,
                text:thisStage.TextAfter,
                replyMarkup: new InlineKeyboardMarkup(NextButton));
        }
        public static CaseStage GetNextStage(CaseStage current, CallbackQuery query)
        {
            if (query.Data == "MoveNext")
            {
                return Stages[current.NextStage];
            }
            else if(query.Data == "ToBegin")
            {
                return Stages.Stages.Min();
            }
            else if(query.Data == "ToOut")
            {
                return null;
            }
            throw new ArgumentException();
            //Если пользователь нажал далее, то переходим через свойство NextStage, которое либо было изначально
            //установлено, либо установлено вследствие условного перехода.
            //Если пользователь нажал вернуться в начало модуля, то возвращаем первый этап текущего модуля 
            //TODO придумать, что делать с его очками во 2 случае
        }
        public static async Task Move(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            switch (nextStage)
            {
                case CaseStagePoll poll:
                    await botClient.SendPollAsync(
                        chatId: userId,
                        question: poll.TextBefore,
                        options: poll.Options,
                        isAnonymous: false,
                        allowsMultipleAnswers: poll.ManyAnswers,
                        replyMarkup: new InlineKeyboardMarkup(ToFinishButton));
                    break;
                case CaseStageNone none:
                    await botClient.SendTextMessageAsync(userId,
                        none.TextBefore,
                        replyMarkup: StageMenu);
                    break;
                case CaseStageEndModule endStage:

                default:
                    break;
            }
            //В зависимости от типа этапа, бот выдаёт определённый тип сообщения
        }
    }
}
