using Simulator.BotControl;
using Simulator.BotControl.State;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class UserCardCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
			UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
            userState.SetDialogState(DialogState.None);
            await DataBaseControl.UpdateEntity(userId, userState);

            string userCardString = Resources.UserCard;
            User user = await DataBaseControl.GetEntity<User>(userId);
            userCardString += $"\n{user}";
            userCardString += $"\nВаша группа: {user.GroupNumber}";
            userCardString += $"\nВаш telegramID: {user.UserID}\n";
            await botClient.SendTextMessageAsync(chatId: userId,
                text: userCardString,
                replyMarkup: CommandKeyboard.UserCardMenu);
        }
    }
}
