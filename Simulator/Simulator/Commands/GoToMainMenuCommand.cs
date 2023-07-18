using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class GoToMainMenuCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            UserType userType = await DataBaseControl.UserTableCommand.GetUserType(userId);
            switch (userType)
            {
                case UserType.Admin:
                    await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.AdminMenu,
                            replyMarkup: CommandKeyboard.AdminMenu);
                    break;
                case UserType.ClassLeader:
                    await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.UserMenu,
                            replyMarkup: CommandKeyboard.GroupLeaderMenu);
                    break;
                case UserType.User:
                    await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.UserMenu,
                            replyMarkup: CommandKeyboard.UserMenu);
                    break;
                default:
                    break;
            }
        }
    }
}
