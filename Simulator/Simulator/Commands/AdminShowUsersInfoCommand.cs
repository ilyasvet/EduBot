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
        public override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            return Task.Run(() =>
            {
                List<User> users = UserTableCommand.GetListUsers(param);
                string messageWithList = $"{Resources.ShowUsers}\n";
                foreach (User user in users)
                {
                    messageWithList += $"{user}\n";
                }
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: messageWithList,
                            replyMarkup: CommandKeyboard.ToMainMenuAdmin);
            });
        }
    }
}
