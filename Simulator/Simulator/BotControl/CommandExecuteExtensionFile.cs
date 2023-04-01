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
        public static async Task Execute(long userId, ITelegramBotClient botClient, string path)
        {
            await Task.Run(async() =>
            {
                DialogState state = UserTableCommand.GetDialogState(userId);
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.AddingUsersToGroup:
                        await AddNewUsersTable(userId, botClient, path);
                        break;
                    default:
                        break;
                }
            });
        }
        private async static Task AddNewUsersTable(long userId, ITelegramBotClient botClient, string path)
        {
            string callBackMessage = "";
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.ExcelTable)) throw new ArgumentException("Файл должен быть exel");
                string groupNumber = GroupHandler.GetGroupNumberFromPath(path);
                if (!GroupHandler.IsCorrectGroupNumber(groupNumber)) throw new ArgumentException("Неверный формат номера группы");
                if (!GroupTableCommand.HasGroup(groupNumber))
                {
                    GroupHandler.AddGroup(groupNumber);
                    callBackMessage += $"\nГруппа \"{groupNumber}\" была добавлена";
                }
                int count = ExcelHandler.AddUsersFromExcel(path, groupNumber);
                callBackMessage += $"\nДобавлено пользователей в группу \"{groupNumber}\": {count}\n";
                await BotCallBack(userId, botClient, callBackMessage.Insert(0, Resources.SuccessAddGroup));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + callBackMessage);
            }
            finally
            {
                UserTableCommand.SetDialogState(userId, DialogState.None);
                System.IO.File.Delete(path);
                //После обработки файл удаляется
            }
        }
        private async static Task BotCallBack(long userId, ITelegramBotClient botClient, string message)
        {
            await botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: message,
                       replyMarkup: CommandKeyboard.ToMainMenuAdmin);
        }
    }
}