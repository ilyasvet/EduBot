using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.BotControl.State;
using System;
using Simulator.Properties;
using Simulator.Services;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionFile
    {
        public static Task Execute(long userId, ITelegramBotClient botClient, string path)
        {
            return Task.Run(() =>
            {
                DialogState state = UserTableCommand.GetDialogState(userId);
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.AddingUsersToGroup:
                        AddNewUsersTable(userId, botClient, path);
                        break;
                    default:
                        break;
                }
            });
        }
        private static void AddNewUsersTable(long userId, ITelegramBotClient botClient, string path)
        {
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.ExcelTable)) throw new ArgumentException("Файл должен быть exel");
                string groupNumber = GroupHandler.GetGroupNumberFromPath(path);
                if (!Checker.IsCorrectGroupNumber(groupNumber)) throw new ArgumentException("Неверный формат номера группы");
                string callBackMessage = Resources.SuccessAddGroup;
                if (!GroupTableCommand.HasGroup(groupNumber))
                {
                    GroupHandler.AddGroup(groupNumber);
                    callBackMessage += $"\nГруппа \"{groupNumber}\" была добавлена";
                }
                int count = ExcelHandler.AddUsersFromExcel(path, groupNumber);
                callBackMessage += $"\nДобавлено пользователей в группу \"{groupNumber}\": {count}\n";
                BotCallBack(userId, botClient, callBackMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                UserTableCommand.SetDialogState(userId, DialogState.None);
            }
        }
        private static void BotCallBack(long userId, ITelegramBotClient botClient, string message)
        {
            botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: message,
                       replyMarkup: CommandKeyboard.ToMainMenuAdmin);
        }
    }
}