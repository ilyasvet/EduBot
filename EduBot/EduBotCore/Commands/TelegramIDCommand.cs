using EduBot.BotControl;
using EduBot.BotControl.State;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;

namespace EduBot.Commands
{
    public class TelegramIDCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
            string userTelegramIdString = Resources.TelegramId;
            userTelegramIdString += $"\n{userId}";

			UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
			if (userState != null)
			{
				userState.SetDialogState(DialogState.None);
				await DataBaseControl.UpdateEntity(userId, userState);
				await botClient.SendTextMessageAsync(chatId: userId,
                    text: userTelegramIdString,
                    replyMarkup: CommandKeyboard.ToMainMenu);
            }
            else
            {
                string textUnknown = Resources.WelcomeUnknown + $"\n\n{userTelegramIdString}";
                await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: $"{textUnknown}\n{Resources.EnterStart}");
            }
        }
    }
}
