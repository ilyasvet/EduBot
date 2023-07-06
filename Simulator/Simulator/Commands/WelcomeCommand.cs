using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;

namespace Simulator.Commands
{
    public class WelcomeCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                if (UserTableCommand.HasUser(userId))
                {
                    if(!UserTableCommand.IsAdmin(userId))
                    {
                        botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WelcomeKnown,
                            replyMarkup: CommandKeyboard.LogIn);
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WelcomeKnownAdmin,
                            replyMarkup: CommandKeyboard.ToMainMenuAdmin);
                    }
                }
                else
                {
                    botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WelcomeUnknown,
                            replyMarkup: CommandKeyboard.TelegramId);
                }
            });
        }
    }
}
