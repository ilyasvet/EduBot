using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotLibrary;

namespace Simulator.Commands
{
    class RegistrationRequestCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                UserTableCommand.AddUser(userId);
                botClient.SendTextMessageAsync(userId,
                    text: Resources.RegistrationGuide,
                    replyMarkup: CommandKeyboard.EnterName);
            });
        }
    }
}
