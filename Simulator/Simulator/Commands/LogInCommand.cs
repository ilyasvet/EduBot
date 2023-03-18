using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;

namespace Simulator.Commands
{
    class LogInCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, DialogState.EnterPassword);
                botClient.SendTextMessageAsync(userId, Resources.EnterPassword);
            });
        }
    }
}
