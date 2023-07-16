using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminShowUsersInfoCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            List<User> users = await DataBaseControl.UserTableCommand.GetGroupUsers(param);
            string messageWithList = $"{Resources.ShowUsers} {param}\n" +
            $"{Resources.GroupPassword} {await DataBaseControl.GroupTableCommand.GetPassword(param)}\n";
            foreach (User user in users)
            {
                messageWithList += $"{user}\n";
            }
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: messageWithList,
                        replyMarkup: CommandKeyboard.ToGroups);
        }
    }
}
