using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TestSimulator.BotControl
{
    internal class TelegramBotControl
    {
        private TelegramBotClient botClient;

        public TelegramBotControl()
        {
            botClient = new TelegramBotClient("6250154721:AAHNyMELX0pyxhX3KbCpWFI8nSCUFdDtiv8");
        }
        public void TelegramBotController()
        {
            botClient.StartReceiving(
                Update,
                Error
            );
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                if (message.Text.ToLower() == "проверка работоспособности")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Бот отвечает.");
                    return;
                }
            }
        }

        async static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
