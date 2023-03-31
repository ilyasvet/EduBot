using Simulator.Commands;
using Simulator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Simulator.BotControl
{
    internal class TelegramBotControl
    {
        private TelegramBotClient botClient;
        private static Dictionary<string, Command> commandsDictionary;
        private static Dictionary<string, string> accordanceDictionaryTextCommand = new Dictionary<string, string>();
        private static Dictionary<string, string> accordanceDictionaryButtonCommand = new Dictionary<string, string>();

        static TelegramBotControl()
        {
            commandsDictionary = Filler.FillCommandDictionary();
            accordanceDictionaryButtonCommand = Filler.FillAccordanceDictionary("AccordanceButtonCommands.txt");
            accordanceDictionaryTextCommand = Filler.FillAccordanceDictionary("AccordanceTextCommands.txt");
        }
        public TelegramBotControl(string token)
        {
            botClient = new TelegramBotClient(token);
        }
        public void ManagementTelegramBot()
        {
            botClient.StartReceiving(
                Update,
                Error
            );
        }

        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            long userId = 0;
            try
            {
                if (update.Message?.Text != null)
                {
                    string messageText = update.Message.Text;
                    userId = update.Message.Chat.Id;
                    if (Checker.TextIsCommand(accordanceDictionaryTextCommand, messageText))
                    {
                        //Тут бот выполняет команду, введённую пользователем
                        await commandsDictionary[accordanceDictionaryTextCommand[messageText]].Execute(userId, botClient);
                    }
                    else
                    {
                        //Тут бот принимает от пользователя какие-то данные
                        await CommandExecuteExtensionText.Execute(userId, botClient, messageText);
                    }
                }
                else if (update.Message?.Document != null)
                {
                    //Тут бот принимает от пользователя файл
                    userId = update.Message.Chat.Id;
                    Document messageDocument = update.Message.Document;
                    var file = await botClient.GetFileAsync(messageDocument.FileId);

                    //все файлы, посланные боту, хранятся в папке temp
                    string path = $"{AppDomain.CurrentDomain.BaseDirectory}temp\\{messageDocument.FileName}";
                    FileStream fs = new FileStream(path, FileMode.Create);
                    await botClient.DownloadFileAsync(file.FilePath, fs);
                    fs.Dispose();

                    await CommandExecuteExtensionFile.Execute(userId, botClient, path);
                    System.IO.File.Delete(path);
                    //После обработки файл удаляется
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    //Тут бот выполняет команду по нажатию на кнопку
                    CallbackQuery callbackQuery = update.CallbackQuery;
                    userId = callbackQuery.Message.Chat.Id;
                    string data = callbackQuery.Data;
                    string commandWord = callbackQuery.Data.Split('|')[0];
                    string param = Checker.GetCommandCallbackQueryParam(data);
                    await commandsDictionary[accordanceDictionaryButtonCommand[commandWord]].Execute(
                        userId,
                        botClient,
                        param);
                }
                int messageId = update.CallbackQuery != null ?
                    update.CallbackQuery.Message.MessageId :
                    update.Message.MessageId;
                await botClient.DeleteMessageAsync(userId, messageId);
            }
            catch (Exception ex)
            {
                throw new Exception($"{userId} {ex.Message}");
            }
        }
        private async Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            //Все исключения обработаны так, что возвращают в сообщении также userId
            long userId = long.Parse(exception.Message.Split(' ')[0]);
            await commandsDictionary[accordanceDictionaryTextCommand["/skip"]].Execute(
                        userId,
                        botClient,
                        exception.Message.Remove(0, userId.ToString().Length).Trim());
        }
    }
}
