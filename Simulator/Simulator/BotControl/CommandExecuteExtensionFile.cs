using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;
using Simulator.BotControl.State;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using Simulator.Properties;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Simulator.Models;
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
                string groupNumber = GetGroupNumber(path);
                if (!Checker.IsCorrectGroupNumber(groupNumber)) throw new ArgumentException("Неверный формат номера группы");
                string callBackMessage = Resources.SuccessAddGroup;
                if (!GroupTableCommand.HasGroup(groupNumber))
                {
                    AddGroup(groupNumber);
                    callBackMessage += $"\nГруппа \"{groupNumber}\" была добавлена";
                }
                int count = AddUsers(path, groupNumber);
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
        private static int AddUsers(string path, string groupNumber)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            int count = 0;
            try
            {
                workbooks = excelApp.Workbooks;
                workbook = excelApp.Workbooks.Open(path);
                Excel.Worksheet worksheet = workbook.Worksheets[1];

                // Получаем диапазон используемых на странице ячеек
                Excel.Range usedRange = worksheet.UsedRange;
                // Получаем строки в используемом диапазоне
                Excel.Range urRows = usedRange.Rows;
                // Получаем столбцы в используемом диапазоне
                Excel.Range urColums = usedRange.Columns;

                if (urColums.Count != 3) throw new ArgumentException("В таблице должно быть только 3 столбца!");

                count = AddUsersIterative(worksheet, urRows.Count, groupNumber);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                workbook.Close();
                workbooks.Close();
                excelApp.Quit();
                KillProcess("EXCEL");
            }
            return count;
        }
        private static void BotCallBack(long userId, ITelegramBotClient botClient, string message)
        {
            botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: message,
                       replyMarkup: CommandKeyboard.ToMainMenuAdmin);
        }
        private static int AddUsersIterative(Worksheet worksheet, int rowsCount, string groupNumber)
        {
            int count = 0;
            for (int i = 1; i <= rowsCount; i++)
            {
                long userTelegramId = (long)worksheet.Cells[i, 3].Value;
                if(UserTableCommand.HasUser(userTelegramId))
                {
                    continue;
                }
                string userName = worksheet.Cells[i, 1].Value;
                string userSurname = worksheet.Cells[i, 2].Value;

                Models.User user = new Models.User(userTelegramId, userName, userSurname);
                user.GroupNumber = groupNumber;
                UserTableCommand.AddUser(user);
                count++;
            }
            return count;
        }
        private static void AddGroup(string groupNumber)
        {
            Models.Group group = new Models.Group(groupNumber);
            SetGroupPassword(group);
            GroupTableCommand.AddGroup(group);
        }
        private static string GetGroupNumber(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1).Split('.')[0];
        }
        private static void SetGroupPassword(Group group)
        {
            string password = "";
            int passwordLenght = 6;
            Random rnd = new Random();
            for(int i = 0; i < passwordLenght; i++)
            {
                password += (char)(rnd.Next(25)+97);
            }
            group.Password = password;
        }
        private static void KillProcess(string name)
        { 
            GC.Collect();
            Process[] List;
            List = Process.GetProcessesByName(name);
            foreach (Process proc in List)
            {
                proc.Kill();
            }
        }
    }
}