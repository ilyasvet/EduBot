using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class TelegramIDCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            string userTelegramIdString = Resources.TelegramId;
            userTelegramIdString += $"\n{userId}";

            if (await DataBaseControl.UserTableCommand.HasUser(userId))
            {
                await DataBaseControl.UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                if (await DataBaseControl.UserTableCommand.GetUserType(userId) == Models.UserType.Admin)
                {
                    await botClient.SendTextMessageAsync(chatId: userId,
                        text: userTelegramIdString,
                        replyMarkup: CommandKeyboard.AdminMenu);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                                   chatId: userId,
                                   text: userTelegramIdString,
                                   replyMarkup: CommandKeyboard.UserMenu);
                }
            }
            else
            {
                string textUnknown = Resources.WelcomeUnknown + $"\n\n{userTelegramIdString}";
                await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: $"{textUnknown}\n{Resources.EnterStart}");
            }
        }
    }
}
