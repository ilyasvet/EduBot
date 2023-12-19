using Simulator.BotControl;
using Simulator.Properties;
using SimulatorCore.Models.DbModels;
using Telegram.Bot;

namespace Simulator.Commands
{
    public class WelcomeCommand : Command
    {
        public override async Task Execute(long userId, ITelegramBotClient botClient, string param = "")
        {
			UserState userState = await DataBaseControl.GetEntity<UserState>(userId);
			if (userState != null)
			{
                if (userState.GetUserType() != UserType.Admin)
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
                await botClient.SendTextMessageAsync(
                        chatId: userId,
                        text: Resources.WelcomeUnknown,
                        replyMarkup: CommandKeyboard.TelegramId);
            }
        }
    }
}
