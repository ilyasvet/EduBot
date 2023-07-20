using Simulator.BotControl;
using Simulator.Models;
using Simulator.Properties;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Simulator.Commands
{
    public class GoToMainMenuCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            UserType userType = await DataBaseControl.UserTableCommand.GetUserType(userId);
            int startDialogMessageId = await DataBaseControl.UserFlagsTableCommand.GetMessageStartDialogId(userId);
            int endDialogMessageId = 0;
            switch (userType)
            {
                case UserType.Admin:
                    endDialogMessageId = (await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.AdminMenu,
                            replyMarkup: CommandKeyboard.AdminMenu)).MessageId;
                    break;
                case UserType.ClassLeader:
                    endDialogMessageId = (await botClient.SendTextMessageAsync(
                            chatId: userId,
                            text: Resources.UserMenu,
                            replyMarkup: CommandKeyboard.GroupLeaderMenu)).MessageId;
                    break;
                case UserType.User:
                    endDialogMessageId = (await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.UserMenu,
                        replyMarkup: CommandKeyboard.UserMenu)).MessageId;
                    break;
                default:
                    break;
            }
            endDialogMessageId--;
            if (startDialogMessageId != 0)
            {
                while (startDialogMessageId < endDialogMessageId)
                {
                    try
                    {
                        await botClient.DeleteMessageAsync(userId, startDialogMessageId);
                    }
                    catch { }
                    
                    startDialogMessageId++;
                }
            }
            await DataBaseControl.UserFlagsTableCommand.SetMessageStartDialogId(userId, 0);
        }
    }
}
