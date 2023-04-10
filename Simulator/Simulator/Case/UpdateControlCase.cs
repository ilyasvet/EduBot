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
            //массив вопросов[point].MoveTo(query)
        }
    }
}
