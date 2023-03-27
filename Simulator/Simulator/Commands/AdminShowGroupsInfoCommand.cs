using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminShowGroupsInfoCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                List<Group> groups = GroupTableCommand.GetAllGroups();
                string messageWithList = $"{Resources.ShowGroups}\n{Resources.ShowGroupsFormat}\n";
                foreach (Group group in groups)
                {
                    messageWithList += $"{group}\n";
                }
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: messageWithList,
                            replyMarkup: CommandKeyboard.ToMainMenuAdmin);
            });
        }
    }
}
