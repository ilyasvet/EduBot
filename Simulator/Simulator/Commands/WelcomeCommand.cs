using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;

namespace Simulator.Commands
{
    class WelcomeCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
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
                            text: Resources.WelcomeUnknown);
                }
            });
        }
    }
}
