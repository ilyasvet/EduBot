using Simulator.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotLibrary;

namespace Simulator.BotControl
{
    internal class TelegramBotControl
    {
        private TelegramBotClient botClient;
        private Dictionary<string, Command> commandsDictionary;
        private Dictionary<string, string> accordanceDictionary;

        private string serverName;
        private string dataBaseName;

        public TelegramBotControl(string token, string server, string dataBase)
        {
            botClient = new TelegramBotClient(token);
            serverName = server;
            dataBaseName = dataBase;
            FillCommandDictionary();
            FillAccordanceDictionary();
        }
        public void ManagementTelegramBot()
        {
            botClient.StartReceiving(
                Update,
                Error
            );
        }

        async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                Tuple<string, CommandParameters> commandDeclarator = TransformMessageToCommand(message);
                commandsDictionary[commandDeclarator.Item1].Execute(commandDeclarator.Item2);
            }
        }

        async Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private Tuple<string, CommandParameters> TransformMessageToCommand(Message message)
        {
            long senderId = message.Chat.Id;
            bool isAdmin = false;
            using (var connection = new LocalSqlDbConnection(serverName, dataBaseName))
            {
                using (var command = new UserTableCommand(connection))
                {
                    isAdmin = command.IsAdmin(senderId);
                }
            }
            string messageText = message.Text;
            string commandName = messageText.Split(' ')[0];
            messageText = messageText.Remove(0, commandName.Length).Trim();
            
            CommandParameters parameters = new CommandParameters(senderId, isAdmin, messageText);
            commandName = accordanceDictionary[commandName];
            return new Tuple<string, CommandParameters>(commandName, parameters);
        }
        private void FillCommandDictionary()
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
        private void FillAccordanceDictionary()
        {
            accordanceDictionary = new Dictionary<string, string>();
            accordanceDictionary.Add("show", "ShowUsersInfoCommandA");
            accordanceDictionary.Add("accept", "AcceptRequestCommandA");
            accordanceDictionary.Add("login", "LogInCommand");
            accordanceDictionary.Add("registration", "RegistrationRequestCommand");
            accordanceDictionary.Add("welcome", "WelcomeCommand");
        }
    }
}
