using Simulator.BotControl.State;
using Simulator.Models;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionText
    {
        public static async Task Execute(long userId, ITelegramBotClient botClient, string message)
        {
            await Task.Run(async () =>
            {
                DialogState state = await DataBaseControl.UserTableCommand.GetDialogState(userId);
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.EnterPassword:
                        await CheckPassword(userId, botClient, message);
                        break;
                    default:
                        break;
                }
            });
        }

        private static async Task CheckPassword(long userId, ITelegramBotClient botClient, string password)
        {
            User user = await DataBaseControl.UserTableCommand.GetUserById(userId);
            string groupPassword = await DataBaseControl.GroupTableCommand.GetPassword(user.GroupNumber);
            if(password == groupPassword)
            {
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
                await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.RightPassword,
                            replyMarkup: CommandKeyboard.ToMainMenuUser);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WrongPassword);
            }
        }
    }
}
