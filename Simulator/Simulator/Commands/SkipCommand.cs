using Simulator.BotControl;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

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
                                replyMarkup: CommandKeyboard.ToMainMenu);
                }
                else
                {
                    IReplyMarkup markup;
                    await DataBaseControl.UserCaseTableCommand.SetOnCourse(userId, false);

                    if (await DataBaseControl.UserTableCommand.IsLogedIn(userId))
                    {
                        markup = CommandKeyboard.ToMainMenu;
                    }
                    else
                    {
                        markup = CommandKeyboard.LogIn;
                    }

                    await botClient.SendTextMessageAsync(
                                       chatId: userId,
                                       text: param + "!",
                                       replyMarkup: markup);
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
