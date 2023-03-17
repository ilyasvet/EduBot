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
        LoginName,
        LoginSurname,
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
                    case DialogState.LoginName:
                        break;
                    case DialogState.LoginSurname:
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
