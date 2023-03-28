using Simulator.BotControl.State;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;

namespace Simulator.Commands
{
    public class AdminAddNewUsersCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            return Task.Run(() =>
            {
				UserTableCommand.SetDialogState(userId, DialogState.AddingUsersToGroup);
                botClient.SendTextMessageAsync(userId, Resources.AddNewGroupOfUsers);
            });
        }
    }
}
