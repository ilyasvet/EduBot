using Simulator.BotControl;
using SimulatorCore.Properties;
using SimulatorCore.Models.DbModels;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

using DbUser = SimulatorCore.Models.DbModels.User;

namespace Simulator.Commands
{
    public class AdminShowUsersInfoCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            IReplyMarkup markup = null;
            UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
            string groupNumber = "";
            if(userState.GetUserType() == UserType.ClassLeader)
            {
                DbUser user = await DataBaseControl.GetEntity<DbUser>(userId);
                groupNumber = user.GroupNumber;
                markup = CommandKeyboard.ToMainMenu;
            }
            else
            {
                groupNumber = param;
                markup = CommandKeyboard.ToGroups;
			}

            Group group = await DataBaseControl.GetEntity<Group>(groupNumber);

            var users = (await DataBaseControl.GetCollection<DbUser>()).Where(u => u.GroupNumber == group.GroupNumber);

			string messageWithList = $"{Resources.ShowUsers} {group.GroupNumber}\n" +
            $"{Resources.GroupPassword} {group.Password}\n";
            foreach (DbUser user in users)
            {
                messageWithList += $"{user}\n";
            }

            await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: messageWithList,
                        replyMarkup: markup);
        }
    }
}
