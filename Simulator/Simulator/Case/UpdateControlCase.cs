using Simulator.Models;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Simulator.Case
{
    internal static class UpdateControlCase
    {
        public static async Task MessageHandlingCase(Message message, ITelegramBotClient botClient)
        {
   
        }
        public static async Task CallbackQueryHandlingCase(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
            int point = UserCaseTableCommand.GetPoint(userId);
            CaseStage nextStage = StagesControl.Stages[StagesControl.Stages[point].NextStage];

            UserCaseTableCommand.SetPoint(userId, nextStage.Number);

            await StagesControl.MoveNext(userId, nextStage, botClient);
        }
        public static async Task PollAnswerHandlingCase(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
            int point = UserCaseTableCommand.GetPoint(userId);
            CaseStagePoll thisStage = StagesControl.Stages[point] as CaseStagePoll;

            StagesControl.SetStageForMove(thisStage, answer.OptionIds);

            double rate = StagesControl.CalculateRatePoll(thisStage, answer.OptionIds);
            string currentUserRate = UserCaseTableCommand.GetRate(userId);
            currentUserRate += $"|{rate}";
            UserCaseTableCommand.SetRate(userId, currentUserRate);

            await botClient.SendTextMessageAsync(userId, thisStage.TextAfter); //кнопка для перехода к следующему этапу
        }
    }
}
