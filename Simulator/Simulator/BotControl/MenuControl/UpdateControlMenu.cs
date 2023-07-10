using Simulator.Commands;
using Simulator.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                string messageText = message.Text;
                if (Checker.TextIsCommand(accordanceDictionaryTextCommand, messageText))
                {
                    //Тут бот выполняет команду, введённую пользователем
                    await commandsDictionary[accordanceDictionaryTextCommand[messageText]].Execute(userId, botClient);
                }
                else
                {
                    //Тут бот принимает от пользователя какие-то данные
                    //Обработка идёт на основе DialogState отправителя
                    await CommandExecuteExtensionText.Execute(userId, botClient, messageText);
                }
            }
            else if (message.Document != null)
            {
                //Тут бот принимает от пользователя файл
                Document messageDocument = message.Document;

                //скачивается файл на сервер и выдаётся путь к нему
                string path = await BotHandler.FileHandle(botClient, messageDocument);

                //Обработка идёт на основе DialogState отправителя
                await CommandExecuteExtensionFile.Execute(userId, botClient, path);
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
            }
            catch(KeyNotFoundException) { }
        }
    }
}
