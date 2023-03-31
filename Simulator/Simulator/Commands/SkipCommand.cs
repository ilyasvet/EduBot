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
                if (UserTableCommand.IsAdmin(userId))
                {
                    botClient.SendTextMessageAsync(
                                chatId: userId,
                                text: param,
                                replyMarkup: CommandKeyboard.AdminMenu);
                }
                else
                {
                    botClient.SendTextMessageAsync(
                                   chatId: userId,
                                   text: param,
                                   replyMarkup: CommandKeyboard.UserMenu);
                }
            });
        }
    }
}
