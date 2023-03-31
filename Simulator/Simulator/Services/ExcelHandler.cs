using Microsoft.Office.Interop.Excel;
using Simulator.TelegramBotLibrary;
using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace Simulator.Services
{
    internal static class ExcelWorker
    {
        public static int AddUsers(string path, string groupNumber)
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
            catch
            {
                throw;
            }
            finally
            {
                workbook.Close();
                workbooks.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL");
            }
            return count;
        }
        private static int AddUsersIterative(Worksheet worksheet, int rowsCount, string groupNumber)
        {
            int count = 0;
            for (int i = 1; i <= rowsCount; i++)
            {
                long userTelegramId;
                try
                {
                    userTelegramId = (long)worksheet.Cells[i, 3].Value;
                }
                catch
                {
                    throw new ArgumentException("TelegramId должен быть целым числом");
                }
                if (UserTableCommand.HasUser(userTelegramId))
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
    }
}
