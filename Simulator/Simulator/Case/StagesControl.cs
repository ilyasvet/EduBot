using Simulator.BotControl;
using Simulator.Models;
using Simulator.Services;
using Simulator.TelegramBotLibrary;
using System;
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
        public static bool Make()
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}temp\\caseinfo.case";
            if (System.IO.File.Exists(path))
            {
                CaseConverter.FromFile(path);
                return true;
            }
            return false;
        }
        public static double CalculateRatePoll(CaseStagePoll stage, int[] answers)
        {
            double rate = 0;
            
            int count = answers.Length;
            if(stage.Limit < count && stage.Limit != 0)
            {
                count = stage.Limit;
                //Штраф за превышение кол-ва ответов
                rate -= stage.Fine * (count - stage.Limit);
            }
            for (int i = 0; i < count;i++)
            {
                rate += stage.PossibleRate[answers[i]];
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
        public static CaseStage GetNextStage(CaseStage current, CallbackQuery query)
        {
            if (query.Data == "MoveNext")
            {
                return Stages[current.NextStage];
            }
            else if(query.Data == "ToBegin")
            {
                UserCaseTableCommand.SetRate(query.From.Id, 0);
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
                        replyMarkup: new InlineKeyboardMarkup(CommandKeyboard.ToFinishButton));
                    break;
                case CaseStageNone none:
                    await botClient.SendTextMessageAsync(userId,
                        none.TextBefore,
                        replyMarkup: CommandKeyboard.StageMenu);
                    break;
                case CaseStageEndModule endStage:
                    var resultsCallback = EndStageCalc.GetResultOfModule(endStage, userId);
                    await botClient.SendTextMessageAsync(userId,
                        resultsCallback.Item1,
                        replyMarkup: resultsCallback.Item2);
                    break;
                default:
                    break;
            }
            //В зависимости от типа этапа, бот выдаёт определённый тип сообщения
        }
    }
}
