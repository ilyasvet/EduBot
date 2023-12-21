using Simulator.BotControl;
using SimulatorCore.Properties;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class AdminShowGroupsInfoCommand : Command
    {
        public async override Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            bool all = false;
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
					break;
				default:
                    break;
            }
            await CommandKeyboard.MakeGroupList(param, all);
            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.ShowGroups,
                        replyMarkup: CommandKeyboard.GroupsList);
        }
    }
}
