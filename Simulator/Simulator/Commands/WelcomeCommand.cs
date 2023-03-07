using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotLibrary;

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
                    if (UserTableCommand.IsRegistered(userId))
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
                            text: Resources.WeclomeUnregistered);
                    }
                }
                else
                {
                    botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.WelcomeUnknown,
                            replyMarkup: CommandKeyboard.Registration);
                }
            });
        }
    }
}
