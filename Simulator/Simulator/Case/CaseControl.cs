using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.Case
{
    internal static class CaseControl
    {
        public static async Task CaseCallback(long userId, Update update, ITelegramBotClient botClient)
        {
            int userPoint = UserCaseTableCommand.GetPoint(userId);

        }
    }
}
