using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotLibrary;

namespace Simulator.Commands
{
    public class EnterNameCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, DialogState.LoginName);
                botClient.SendTextMessageAsync(userId,
                    text: Resources.LoginName);
            });
        }
    }
    public class EnterSurnameCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient)
        {
            return Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, DialogState.LoginSurname);
                botClient.SendTextMessageAsync(userId,
                    text: Resources.LoginSurname);
            });
        }
    }
}