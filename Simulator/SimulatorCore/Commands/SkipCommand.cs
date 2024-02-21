using Simulator.BotControl;
using Simulator.BotControl.State;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simulator.Commands
{
    public class SkipCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
            if (userState != null)
            {
                userState.SetDialogState(DialogState.None);
                await DataBaseControl.UpdateEntity(userId, userState);
                if (userState.GetUserType() == UserType.Admin)
                {
                    await botClient.SendTextMessageAsync(
                                chatId: userId,
                                text: param + "\nПереход в меню",
                                replyMarkup: CommandKeyboard.ToMainMenu);
                }
                else
                {
                    IReplyMarkup markup;
                    UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
                    userFlags.CurrentCourse = null;
                    await DataBaseControl.UpdateEntity(userId, userFlags);

                    if (userState.LogedIn)
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
