using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminShowUsersInfoCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                List<User> users = UserTableCommand.GetGroupUsers(param);
                string messageWithList = $"{Resources.ShowUsers} {param}\n" +
                $"{Resources.GroupPassword} {GroupTableCommand.GetPassword(param)}\n";
                foreach (User user in users)
                {
                    messageWithList += $"{user}\n";
                }
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: messageWithList,
                            replyMarkup: CommandKeyboard.GroupListUsers);
            });
        }
    }
}
