using Simulator.BotControl;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class SkipCommand : Command
    {
        public override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            return Task.Run(() =>
            {
                UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.AdminMenu,
                            replyMarkup: CommandKeyboard.AdminMenu);
            });
        }
    }
}
