using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.BotControl.State;
using System;
using Simulator.Properties;
using Simulator.Services;
using Simulator.Case;
using Telegram.Bot.Types.InputFiles;
using System.IO.Compression;
using System.IO;
using Simulator.Models;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionFile
    {
        public static async Task Execute(long userId, ITelegramBotClient botClient, string path, int messageId)
        {
            await Task.Run(async () =>
            {
                DialogState state = await DataBaseControl.UserTableCommand.GetDialogState(userId);
                try
                {
                    switch (state)
                    {
                        case DialogState.None:
                            break;
                        case DialogState.AddingUsersToGroup:
                            await AddNewUsersTable(userId, botClient, path, messageId - 1);
                            break;
                        case DialogState.AddingCase:
                            await AddCase(userId, botClient, path, messageId - 1);              
                            break;
                        case DialogState.CreatingCase:
                            await CreateCase(userId, botClient, path, messageId - 1);
                            break;
                        default:
                            await BotCallBack(userId, botClient, Resources.WrongArgumentMessage);
                            break;
                    }
                }
                finally
                {
                    await botClient.DeleteMessageAsync(userId, messageId);
                }
            });
        }

        private async static Task CreateCase(long userId, ITelegramBotClient botClient, string path, int messageId)
        {
            string fileCasePath = string.Empty;
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.ExcelTable))
                {
                    await BotCallBack(userId, botClient, "File must be excel");
                    return;
                }
                fileCasePath = await ExcelHandler.CreateCaseAsync(path);
                await BotCallBackWithFile(userId, botClient, fileCasePath);
                await botClient.DeleteMessageAsync(userId, messageId);
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
            }
            catch (ArgumentException ex)
            {
                await BotCallBack(userId, botClient, ex.Message);
            }
            finally
            {
                File.Delete(path);
            }
        }

        private async static Task AddCase(long userId, ITelegramBotClient botClient, string path, int messageId)
        {
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.Case))
                {
                    await BotCallBack(userId, botClient, "Файл должен быть .zip");
                    return;
                }

                StagesControl.DeleteCaseFiles(); //Удаляем старые файлы перед добавлением новых
                ZipFile.ExtractToDirectory(path, ControlSystem.caseDirectory);

                if (StagesControl.Make())
                {
                    await BotCallBack(userId, botClient, Resources.AddCaseSuccess); //сообщение об успехе операции
                }
                else
                {
                    await BotCallBack(userId, botClient, Resources.FileCaseNotFound);
                }
                await botClient.DeleteMessageAsync(userId, messageId);
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
            }
            finally
            {
                File.Delete(path);
            }
        }

        private async static Task AddNewUsersTable(long userId, ITelegramBotClient botClient, string path, int messageId)
        {
            string callBackMessage = "";
            string groupNumber = null;
            try
            {
                if (!Checker.IsCorrectFileExtension(path, FileType.ExcelTable))
                {
                    await BotCallBack(userId, botClient, "Файл должен быть exel");
                    return;
                }

                UserType senderType = await DataBaseControl.UserTableCommand.GetUserType(userId);
                if (senderType == UserType.ClassLeader)
                {
                    groupNumber = await DataBaseControl.UserTableCommand.GetGroupNumber(userId);
                    // Номер группы = номер группы старосты
                    // В этом случае группа уже существует, и она корректна
                }
                else if (senderType == UserType.Admin)
                {
                    groupNumber = Path.GetFileNameWithoutExtension(path);
                    // Получаем номер группы из названия файла

                    if (!GroupHandler.IsCorrectGroupNumber(groupNumber))
                    {
                        await BotCallBack(userId, botClient, "Неверный формат номера группы");
                        return;
                    }

                    if (await GroupHandler.AddGroup(groupNumber)) // Вернёт true, если добавили группу
                    {
                        callBackMessage += $"\nГруппа \"{groupNumber}\" была добавлена";
                    }
                }

                int count = await ExcelHandler.AddUsersFromExcel(path, groupNumber);
                // Добавляем пользователей из файла в группу
               
                callBackMessage += $"\nДобавлено пользователей в группу \"{groupNumber}\": {count}\n";
                await BotCallBack(userId, botClient, callBackMessage.Insert(0, Resources.SuccessAddGroup));
                await botClient.DeleteMessageAsync(userId, messageId);
                await DataBaseControl.UserTableCommand.SetDialogState(userId, DialogState.None);
            }
            catch (ArgumentException ex)
            {
                await BotCallBack(userId, botClient, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + callBackMessage);
            }
            finally
            {
                File.Delete(path);
            }
        }
        private async static Task BotCallBack(long userId, ITelegramBotClient botClient, string message)
        {
            await botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: message,
                       replyMarkup: CommandKeyboard.ToMainMenu);
        }
        private async static Task BotCallBackWithFile(long userId, ITelegramBotClient botClient, string filePath)
        {
            using (Stream fs = new FileStream(filePath, FileMode.Open))
            {
                await botClient.SendDocumentAsync(
                    chatId: userId,
                    document: new InputOnlineFile(fs, filePath),
                    replyMarkup: CommandKeyboard.ToMainMenu
                    );
                File.Delete(filePath);
            }
        }
    }
}