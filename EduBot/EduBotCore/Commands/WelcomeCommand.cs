using EduBot.BotControl;
using EduBotCore.Properties;
using EduBotCore.Models.DbModels;
using Telegram.Bot;
using EduBotCore.DbLibrary.StatsTableCommand;

namespace EduBot.Commands
{
    public class WelcomeCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
			UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
			if (userState != null)
			{
				if (userState.GetUserType() == UserType.Guest)
				{
					await botClient.SendTextMessageAsync(
									   chatId: userId,
									   text: Resources.WelcomeUnknown,
									   replyMarkup: CommandKeyboard.ToMainMenu);
				}
				else if (userState.GetUserType() != UserType.Admin)
                {
                    await botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: Resources.WelcomeKnown,
                       replyMarkup: CommandKeyboard.LogIn);
                }      
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.WelcomeKnownAdmin,
                        replyMarkup: CommandKeyboard.ToMainMenu);
                }
            }
            else
            {
				UserStatsControl usc = new UserStatsControl();
				await usc.AddGuestUserToAllTables(userId);
				await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.WelcomeUnknown,
                        replyMarkup: CommandKeyboard.ToMainMenu);
            }
        }
    }
}
