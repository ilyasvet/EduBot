using Simulator.BotControl;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class TelegramIDCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                string userTelegramIdString = Resources.TelegramId;
                userTelegramIdString += $"\n{userId}";

                if (UserTableCommand.HasUser(userId))
                {
                    UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                    if (UserTableCommand.IsAdmin(userId))
                    {
                        botClient.SendTextMessageAsync(chatId: userId,
                            text: userTelegramIdString,
                            replyMarkup: CommandKeyboard.AdminMenu);
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(
                                       chatId: userId,
                                       text: userTelegramIdString,
                                       replyMarkup: CommandKeyboard.UserMenu);
                    }
                }
                else
                {
                    string textUnknown = Resources.WelcomeUnknown + $"\n\n{userTelegramIdString}";
                    botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: $"{textUnknown}\n{Resources.EnterStart}");
                }
            });
        }
    }
}
