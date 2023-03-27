using Simulator.BotControl.State;
using Simulator.Models;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionText
    {
        public static Task Execute(long userId, ITelegramBotClient botClient, string message)
        {
            return Task.Run(() =>
            {
                DialogState state = UserTableCommand.GetDialogState(userId);//TODO схерали Cannot resolve symbol 'DialogState'
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.EnterPassword:
                        CheckPassword(userId, botClient, message);
                        break;
                    default:
                        break;
                }
            });
        }

        private static void CheckPassword(long userId, ITelegramBotClient botClient, string password)
        {
            User user = UserTableCommand.GetUserById(userId);
            string groupPassword = GroupTableCommand.GetPassword(user.GroupId);
            if(password == groupPassword)
            {
                UserTableCommand.SetDialogState(userId, DialogState.None);
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.RightPassword,
                            replyMarkup: CommandKeyboard.ToMainMenuUser);
            }
            else
            {
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WrongPassword);
            }
        }
    }
}
