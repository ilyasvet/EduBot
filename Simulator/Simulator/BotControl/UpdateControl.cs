using Simulator.Case;
using Simulator.Commands;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.BotControl
{
    internal static class UpdateControl
    {
        public static async Task MessageHandling(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;
            int messageId = message.MessageId;
            try
            {
                if (UserCaseTableCommand.IsOnCourse(userId))
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
            finally //Удаляется 2 сообщения (бота и пользователя)
            {
                try
                {
                    await botClient.DeleteMessageAsync(userId, messageId - 1);
                    await botClient.DeleteMessageAsync(userId, messageId);
                }
                catch { }
            }
        }
        public static async Task CallbackQueryHandling(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
            try
            {
                if (UserCaseTableCommand.IsOnCourse(userId))
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
            finally //Удаляется только сообщение бота
            {
                try
                {
                    await botClient.DeleteMessageAsync(userId, query.Message.MessageId);
                }
                catch { }
            }
        }
        public static async Task PollAnswerHandling(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
            try
            {     
                await UpdateControlCase.PollAnswerHandlingCase(answer, botClient);
            }
            catch (Exception ex)
            {
                await ErrorHandling(userId, botClient, ex);
            }
        }
        private static async Task ErrorHandling(long userId, ITelegramBotClient botClient, Exception ex)
        {
            SkipCommand skip = new SkipCommand();
            await skip.Execute(
                            userId,
                            botClient,
                            ex.Message + "\nПереход в меню...");
        }
    }
}
