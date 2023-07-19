using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class UserCardCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await DataBaseControl.UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
            string userCardString = Resources.UserCard;
            User user = await DataBaseControl.UserTableCommand.GetUserById(userId);
            userCardString += $"\n{user}";
            userCardString += $"\nВаша группа: {user.GroupNumber}\n";
            await botClient.SendTextMessageAsync(chatId: userId,
                text: userCardString,
                replyMarkup: CommandKeyboard.UserCardMenu);
        }
    }
}
