using Simulator.Case;
using Simulator.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Simulator.Services;
using SimulatorCore.Models.DbModels;

namespace Simulator.BotControl
{
    internal static class UpdateControl
    {
        public static async Task MessageHandling(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;
            try
            {
                UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
                if (userFlags != null && userFlags.CurrentCourse != null)
                {
                    await UpdateControlCase.MessageHandlingCase(message, botClient);
                }
                else
                {
                    await UpdateControlMenu.MessageHandlingMenu(message, botClient);
                }
            }
            catch(Exception ex)
            {
                await ErrorHandling(userId, botClient, ex);
            }
        }
        public static async Task CallbackQueryHandling(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
            try
			{
				UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
				if (userFlags != null && userFlags.CurrentCourse != null)
				{
                    await UpdateControlCase.CallbackQueryHandlingCase(query, botClient);
                }
                else
                {
                    await UpdateControlMenu.CallbackQueryHandlingMenu(query, botClient);
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling(userId, botClient, ex);
            }
        }
        public static async Task PollAnswerHandling(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
            try
			{
				UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
				if (userFlags.CurrentCourse != null)
				{
                    await UpdateControlCase.PollAnswerHandlingCase(answer, botClient);
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling(userId, botClient, ex);
            }
        }
        private static async Task ErrorHandling(long userId, ITelegramBotClient botClient, Exception inerException)
        {
            SkipCommand skip = new();
            ControlSystem.ShowExceptionConsole(inerException);
            try
            {
                await skip.Execute(
                                userId,
                                botClient,
                                inerException.Message);
            }
            catch(Exception externalException)
            {
                await botClient.SendTextMessageAsync(chatId: userId,
                            text: "Внутренняя ошибка: " + inerException.Message + 
                            "\nОшибка при обработке:" + externalException.Message +
                            "Enter /Start");
            }
        }
    }
}
