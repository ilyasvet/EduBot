using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminShowGroupsInfoCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await CommandKeyboard.MakeGroupList();
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.ShowGroups,
                        replyMarkup: CommandKeyboard.GroupsList);
        }
    }
}
