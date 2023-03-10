using Simulator.Properties;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotLibrary;

namespace Simulator.BotControl
{
    public enum DialogState
    {
        None,
        RegistrationName,
        RegistrationSurname,
    }
    public static class CommandExecuteExtension
    {
        public static Task Execute(long userId, ITelegramBotClient botClient, string message)
        {
            return Task.Run(() =>
            {
                DialogState state = UserTableCommand.GetDialogState(userId);
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.RegistrationName:
                        AddUserName(userId, botClient, message);
                        break;
                    case DialogState.RegistrationSurname:
                        AddUserSurname(userId, botClient, message);
                        break;
                    default:
                        break;
                }
            });
        }

        private static void AddUserSurname(long userId, ITelegramBotClient botClient, string message)
        {
            //Проверка
            UserTableCommand.AddSurname(userId, message);

            UserTableCommand.SetDialogState(userId, DialogState.None);
            botClient.SendTextMessageAsync(userId,
                text: Resources.RegistrationSurnameSuccess);
        }

        private static void AddUserName(long userId, ITelegramBotClient botClient, string message)
        {
            //Проверка
            UserTableCommand.AddName(userId, message);

            UserTableCommand.SetDialogState(userId, DialogState.None);
            botClient.SendTextMessageAsync(userId,
                text: Resources.RegistrationNameSuccess,
                replyMarkup: CommandKeyboard.EnterSurname);
        }
    }
}
