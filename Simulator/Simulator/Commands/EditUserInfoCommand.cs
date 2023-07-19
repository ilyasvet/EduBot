using Simulator.BotControl;
using Simulator.BotControl.State;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    internal class EditUserInfoCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.EditingUserInfo);
            await botClient.SendTextMessageAsync(
                chatId: userId,
                text: Resources.EnterUserInfo,
                replyMarkup: CommandKeyboard.BackToUserCard);
        }
    }
}
