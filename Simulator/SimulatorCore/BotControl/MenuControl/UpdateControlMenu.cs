using Simulator.Commands;
using Simulator.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Simulator.BotControl
{
    internal static class UpdateControlMenu
    {
        private static Dictionary<string, Command> commandsDictionary;
        private static Dictionary<string, string> accordanceDictionaryTextCommand = new Dictionary<string, string>();
        private static Dictionary<string, string> accordanceDictionaryButtonCommand = new Dictionary<string, string>();
        static UpdateControlMenu()
        {
            commandsDictionary = Filler.FillCommandDictionary();
            accordanceDictionaryButtonCommand = Filler.FillAccordanceDictionaryButton();
            accordanceDictionaryTextCommand = Filler.FillAccordanceDictionaryText();
        }
        public async static Task MessageHandlingMenu(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;
            if (message.Text != null) //Бот принимает сообщение от пользователя
            {
                if (Checker.TextIsCommand(accordanceDictionaryTextCommand, message.Text))
                {
                    //Тут бот выполняет команду, введённую пользователем
                    await commandsDictionary[accordanceDictionaryTextCommand[message.Text]].Execute(userId, botClient);
                    await botClient.DeleteMessageAsync(userId, message.MessageId);
                }
                else
                {
                    //Тут бот принимает от пользователя какие-то данные
                    //Обработка идёт на основе DialogState отправителя
                    await CommandExecuteExtensionText.Execute(userId, botClient, message);
                }
            }
            else if (message.Document != null)
            {
                //Тут бот принимает от пользователя файл
                Document messageDocument = message.Document;

                //скачивается файл на сервер и выдаётся путь к нему
                string path = await BotHandler.FileHandle(botClient, messageDocument);

                //Обработка идёт на основе DialogState отправителя
                await CommandExecuteExtensionFile.Execute(userId, botClient, path, message.MessageId);
            }
        }
        public async static Task CallbackQueryHandlingMenu(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
            string data = query.Data;
            string commandWord = data.Split('|')[0];
            string param = Checker.GetCommandCallbackQueryParam(data);
            try
            {
                await commandsDictionary[accordanceDictionaryButtonCommand[commandWord]].Execute(
                    userId,
                    botClient,
                    param);

                try
                {
                    await botClient.DeleteMessageAsync(userId, query.Message.MessageId);
                }
                catch { }
            }
            catch(KeyNotFoundException)
            {
                // Не все CallbackQuery соответствуют командам
                // В этом случае не делаем ничего
            }
        }
    }
}
