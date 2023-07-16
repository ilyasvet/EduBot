using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class SkipCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            if (await DataBaseControl.UserTableCommand.HasUser(userId))
            {
                await DataBaseControl.UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                if (await DataBaseControl.UserTableCommand.GetUserType(userId) == Models.UserType.Admin)
                {
                    await botClient.SendTextMessageAsync(
                                chatId: userId,
                                text: param + "\nПереход в меню",
                                replyMarkup: CommandKeyboard.AdminMenu);
                }
                else
                {
                    await DataBaseControl.UserCaseTableCommand.SetOnCourse(userId, false);
                    await botClient.SendTextMessageAsync(
                                   chatId: userId,
                                   text: param + "\nВойдите заново...",
                                   replyMarkup: CommandKeyboard.LogIn);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.WelcomeUnknown,
                        replyMarkup: CommandKeyboard.TelegramId);
            }
        }
    }
}
