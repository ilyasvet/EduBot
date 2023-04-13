using Simulator.Models;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
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
            await botClient.SendTextMessageAsync(userId, nextStage.Text);
        }
        public static async Task PollAnswerHandlingCase(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
            int point = UserCaseTableCommand.GetPoint(userId);
            CaseStage thisStage = StagesControl.Stages[point];
            StagesControl.CalculateRate(thisStage, answer.OptionIds);
            
        }
        
    }
}
