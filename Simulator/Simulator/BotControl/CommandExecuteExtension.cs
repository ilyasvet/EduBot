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
            return null;
        }
    }
}
