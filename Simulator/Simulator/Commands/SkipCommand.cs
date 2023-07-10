using Simulator.BotControl;
using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class SkipCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            await Task.Run(() =>
            {
                    if (UserTableCommand.HasUser(userId))
                    {
                        UserTableCommand.SetDialogState(userId, BotControl.State.DialogState.None);
                        if (UserTableCommand.IsAdmin(userId))
                        {
                            botClient.SendTextMessageAsync(
                                        chatId: userId,
                                        text: param + "\nПереход в меню",
                                        replyMarkup: CommandKeyboard.AdminMenu);
                        }
                        else
                        {
                            UserCaseTableCommand.SetOnCourse(userId, false);
                            botClient.SendTextMessageAsync(
                                           chatId: userId,
                                           text: param + "\nВойдите заново...",
                                           replyMarkup: CommandKeyboard.LogIn);
                        }
                    }
                    else
                    {
                        botClient.SendTextMessageAsync(
                                chatId: userId,
                                text: Resources.WelcomeUnknown,
                                replyMarkup: CommandKeyboard.TelegramId);
                    }            
            });
        }
    }
}
