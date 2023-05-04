using Simulator.BotControl.State;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.TelegramBotLibrary;
using Simulator.BotControl;

namespace Simulator.Commands
{
    public class AdminAddNewUsersCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
				UserTableCommand.SetDialogState(userId, DialogState.AddingUsersToGroup);
                botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: Resources.AddNewGroupOfUsers,
                    replyMarkup: CommandKeyboard.ToMainMenuAdmin);
            });
        }
    }
}
