using Microsoft.Office.Interop.Excel;
using Simulator.TelegramBotLibrary;
using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace Simulator.Services
{
    internal static class ExcelHandler
    {
        public static int AddUsersFromExcel(string path, string groupNumber)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            int count = 0;
            try
            {
                workbooks = excelApp.Workbooks; //это хранилище наших файлов, с которыми мы работаем
                workbook = workbooks.Open(path); //открываем файл excel
                Excel.Worksheet worksheet = workbook.Worksheets[1]; //берём 1 страницу (счёт с 1)

                // Получаем диапазон используемых на странице ячеек
                Excel.Range usedRange = worksheet.UsedRange;
                // Получаем строки в используемом диапазоне
                Excel.Range urRows = usedRange.Rows;
                // Получаем столбцы в используемом диапазоне
                Excel.Range urColums = usedRange.Columns;

                if (urColums.Count != 3) throw new ArgumentException("В таблице должно быть только 3 столбца!");

                count = AddUsersIterative(worksheet, urRows.Count, groupNumber); //построчно добавляем пользователей
            }
            catch
            {
                throw;
            }
            finally
            {
                workbook.Close(); //освобождаем неуправляемые ресурсы
                workbooks.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL"); //и завершаем процесс, чтобы он не висел
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
                    userTelegramId = (long)worksheet.Cells[i, 3].Value; //берём из 3 столбца Id
                } // сеlls это матрица всех ячеек ячейки
                catch
                {
                    throw new ArgumentException("TelegramId должен быть целым числом. Строка " + i);
                }
                if (UserTableCommand.HasUser(userTelegramId))
                {
                    continue; //Если пользователь уже есть, повторно не добавляем его
                }
                string userName = worksheet.Cells[i, 1].Value; //Берём имя из 1 столбца
                string userSurname = worksheet.Cells[i, 2].Value; //Фамилию из 2 столбца

                Models.User user; //Создаём пользователя. И свойства проверяют входные данные
                try
                {
                    user = new Models.User(userTelegramId, userName, userSurname);
                }
                catch (Exception ex) //При неправильных данных выдаётся исключение с номером строки с ошибкой
                {
                    throw new ArgumentException(ex.Message + " Строка " + i);
                }
                user.GroupNumber = groupNumber; //Номер группы мы уже проверяли ранее
                UserTableCommand.AddUser(user);
                UserCaseTableCommand.AddUser(userTelegramId);
                count++; //Увеличили счётчик добавленных пользователей
            }
            return count;
        }
    }
}
