using Simulator.Commands;
using Simulator.Properties;
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
            if (update.Message?.Text != null)
            {
                string messageText = update.Message.Text;
                long userId = update.Message.Chat.Id;
                if(TextIsCommand(messageText))
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
                long userId = update.Message.Chat.Id;
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
            if (update.Type == UpdateType.CallbackQuery)
            {
                //Тут бот выполняет команду по нажатию на кнопку
                CallbackQuery callbackQuery = update.CallbackQuery;
                await commandsDictionary[accordanceDictionaryButtonCommand[callbackQuery.Data]].Execute(
                    callbackQuery.Message.Chat.Id,
                    botClient);
            }
        }
        private async Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private bool TextIsCommand(string text)
        {
            if (text.StartsWith("/"))
            {
                if (accordanceDictionaryTextCommand.ContainsKey(text))
                {
                    return true;
                }
            }
            return false;
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
                    string line;
                    do
                    {
                        line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        string[] pair = line.Split(' ');
                        result.Add(pair[0], pair[1]);
                    }
                    while (true);
                }
            }
            return result;
        }
    }
}
