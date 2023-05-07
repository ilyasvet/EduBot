using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.BotControl.State;
using System;
using Simulator.Properties;
using Simulator.Services;
using Simulator.Case;
using System.IO.Compression;
using System.Configuration;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionFile
    {
        public static async Task Execute(long userId, ITelegramBotClient botClient, string path)
        {
            await Task.Run(async() =>
            {
                DialogState state = UserTableCommand.GetDialogState(userId);
                try
                {
                    switch (state)
                    {
                        case DialogState.None:
                            break;
                        case DialogState.AddingUsersToGroup:
                            await AddNewUsersTable(userId, botClient, path);
                            break;
                        case DialogState.AddingCase:
                            await AddCase(userId, botClient, path);
                            break;
                        default:
                            break;
                    }
                }
                finally
                {
                    UserTableCommand.SetDialogState(userId, DialogState.None); //в любом случае скипаем стадию отправки файла
                }
            });
        }

        private async static Task AddCase(long userId, ITelegramBotClient botClient, string path)
        {
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.Case))
                    throw new ArgumentException("Файл должен быть .zip");
                string dir = $"{AppDomain.CurrentDomain.BaseDirectory}" +
                    $"{ConfigurationManager.AppSettings["PathCase"]}";

                StagesControl.DeleteCaseFiles(); //Удаляем старые файлы перед добавлением новых
                ZipFile.ExtractToDirectory(path, dir);
                
                if (StagesControl.Make())
                {
                    await BotCallBack(userId, botClient, Resources.AddCaseSuccess); //сообщение об успехе операции
                }
                else
                {
                    await BotCallBack(userId, botClient, Resources.FileCaseNotFound);
                }
            }
            finally
            {
                System.IO.File.Delete(path);
            }
        }

        private async static Task AddNewUsersTable(long userId, ITelegramBotClient botClient, string path)
        {
            string callBackMessage = "";
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.ExcelTable)) throw new ArgumentException("Файл должен быть exel");
                string groupNumber = GroupHandler.GetGroupNumberFromPath(path); //Получаем номер группы из названия файла
                if (!GroupHandler.IsCorrectGroupNumber(groupNumber)) throw new ArgumentException("Неверный формат номера группы");
                if (!GroupTableCommand.HasGroup(groupNumber)) //Проверяем, есть ли такая группа
                {
                    GroupHandler.AddGroup(groupNumber); //Если нет, то создаём её
                    callBackMessage += $"\nГруппа \"{groupNumber}\" была добавлена";
                }
                int count = ExcelHandler.AddUsersFromExcel(path, groupNumber); //Добавляем пользователей из файла в группу
                callBackMessage += $"\nДобавлено пользователей в группу \"{groupNumber}\": {count}\n";
                await BotCallBack(userId, botClient, callBackMessage.Insert(0, Resources.SuccessAddGroup)); //сообщение об успехе операции
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + callBackMessage);
            }
            finally
            {
                System.IO.File.Delete(path);
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