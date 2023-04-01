using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class UserCardCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                string userCardString = Resources.UserCard;
                User user = UserTableCommand.GetUserById(userId);
                userCardString += $"\n{user}";
                userCardString += $"\nВаша группа: {user.GroupNumber}\n";
                botClient.SendTextMessageAsync(chatId: userId,
                    text: userCardString,
                    replyMarkup: CommandKeyboard.ToMainMenuUser);
            });
        }
    }
}
