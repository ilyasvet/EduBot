using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminShowGroupsInfoCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            return Task.Run(() =>
            {
                CommandKeyboard.MakeGroupList();
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: $"{Resources.ShowGroups}\n{Resources.ShowGroupsFormat}",
                            replyMarkup: CommandKeyboard.GroupsList);
            });
        }
    }
}
