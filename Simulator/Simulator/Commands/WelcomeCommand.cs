using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.Models;

namespace Simulator.Commands
{
    public class WelcomeCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            if (await DataBaseControl.UserTableCommand.HasUser(userId))
            {
                if (await DataBaseControl.UserTableCommand.GetUserType(userId) != UserType.Admin)
                {
                    await botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: Resources.WelcomeKnown,
                       replyMarkup: CommandKeyboard.LogIn);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.WelcomeKnownAdmin,
                        replyMarkup: CommandKeyboard.ToMainMenuAdmin);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.WelcomeUnknown,
                        replyMarkup: CommandKeyboard.TelegramId);
            }
        }
    }
}
