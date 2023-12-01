using Simulator.Case;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Simulator.BotControl
{
    internal class TelegramBotControl
    {
        private TelegramBotClient botClient;
        public TelegramBotControl(string token)
        {
            botClient = new TelegramBotClient(token);
        }
        public void ManageTelegramBot()
        {
            try
            {
                StagesControl.Make();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            botClient.StartReceiving(
                Update,
                Error
            );
        }

        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    //await botClient.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                    await UpdateControl.MessageHandling(update.Message, botClient);
                    break;
                case UpdateType.CallbackQuery:
                    await UpdateControl.CallbackQueryHandling(update.CallbackQuery, botClient);
                    break;
                case UpdateType.PollAnswer:
                    await UpdateControl.PollAnswerHandling(update.PollAnswer, botClient);
                    break;
                default:
                    break;
            }
        }
        private async Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {

        }
    }
}
