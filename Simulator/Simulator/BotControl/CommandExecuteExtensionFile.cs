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
using Telegram.Bot.Types;

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
            string groupNumber = GetGroupNumber(path);
            if (!GroupTableCommand.HasGroup(groupNumber))
            {
                AddGroup(groupNumber);
            }
            AddUsers(path, groupNumber);
            BotCallBack(userId, botClient);
        }
        private static void AddUsers(string path, string groupNumber)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
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

                if (urColums.Count != 3) throw new ArgumentException("В таблице должно быть только 3 столбца!"); //Проверка, что колонок должно быть 3

                AddUsersIterative(worksheet, urRows.Count, groupNumber);
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
        }
        private static void BotCallBack(long userId, ITelegramBotClient botClient)
        {
            UserTableCommand.SetDialogState(userId, DialogState.None);
            botClient.SendTextMessageAsync(
                       chatId: userId,
                       text: Resources.SuccessAddGroup,
                       replyMarkup: CommandKeyboard.ToMainMenuAdmin);
        }
        private static void AddUsersIterative(Worksheet worksheet, int rowsCount, string groupNumber)
        {
            for (int i = 1; i <= rowsCount; i++)
            {
                long userTelegramId = (long)worksheet.Cells[i, 3].Value;
                if(UserTableCommand.HasUser(userTelegramId))
                {
                    continue;
                }
                string userName = worksheet.Cells[i, 1].Value;
                string userSurame = worksheet.Cells[i, 2].Value;

                Models.User user = new Models.User(userTelegramId, userName, userSurame);
                user.GroupNumber = groupNumber;
                UserTableCommand.AddUser(user);
            }
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