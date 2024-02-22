using EduBot.BotControl;
using EduBotCore.Properties;
using Telegram.Bot;

namespace EduBot.Commands
{
    public class AdminShowGroupsInfoCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            bool all = false;
            string message = Resources.ShowGroups;
            switch (param)
            {
                case "ShowUsersInfo":
                    all = false;
                    break;
                case "GetListCourses":
                    all = true;
                    break;
				case "AddToCourse":
					all = false;
                    message = Resources.ShowAllGroups;
					break;
				default:
                    break;
            }
            await CommandKeyboard.MakeGroupList(param, all);
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: message,
                        replyMarkup: CommandKeyboard.GroupsList);
        }
    }
}
