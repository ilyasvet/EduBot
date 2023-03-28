using Simulator.Commands;
using Simulator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            FillCommandDictionary();
            accordanceDictionaryButtonCommand = FillAccordanceDictionary("AccordanceButtonCommands.txt");
            accordanceDictionaryTextCommand = FillAccordanceDictionary("AccordanceTextCommands.txt");
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
                    if (Checker.TextIsCommand(commandsDictionary, messageText))
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
                    await commandsDictionary[accordanceDictionaryButtonCommand[callbackQuery.Data]].Execute(
                        userId,
                        botClient);
                }
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
            await botClient.SendTextMessageAsync(userId, exception.Message.Remove(0, userId.ToString().Length).Trim());
        }        
        private static void FillCommandDictionary()
        {
            commandsDictionary = new Dictionary<string, Command>();

            Type baseType = typeof(Command);
            IEnumerable<Type> listOfSubclasses = Assembly.GetAssembly(baseType)
                .GetTypes()
                .Where(type => type.IsSubclassOf(baseType));

            foreach (Type type in listOfSubclasses)
            {
                Command commandObject = Activator.CreateInstance(type) as Command;
                commandsDictionary.Add(type.Name, commandObject);
            }
        }
        private static Dictionary<string, string> FillAccordanceDictionary(string ResourceName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
            path += $"\\Properties\\{ResourceName}";
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] pair = line.Split(' ');
                        result.Add(pair[0], pair[1]);
                        line = sr.ReadLine();
                    }
                }
            }
            return result;
        }
    }
}
