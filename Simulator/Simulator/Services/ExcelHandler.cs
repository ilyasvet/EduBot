using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Simulator.Models;
using Simulator.TelegramBotLibrary;
using System;
using System.IO;
using Telegram.Bot.Types.Enums;
using Excel = Microsoft.Office.Interop.Excel;

namespace Simulator.Services
{
    internal static class ExcelHandler
    {
        public static string CreateCase(string path)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
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

                using (Stream fs = new FileStream("case.json", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteAsync(AddCaseStageIterative(worksheet, urRows.Count));
                        return "case.json";
                    }
                }
                
            }
            finally
            {
                workbook.Close(); //освобождаем неуправляемые ресурсы
                workbooks.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL"); //и завершаем процесс, чтобы он не висел
            }
        }

        private static string AddCaseStageIterative(Worksheet worksheet, int count)
        {
            JObject stageListJObject = new JObject();
            for (int i = 2; i <= count; i++)
            {
                string stageType;
                stageType = worksheet.Cells[i, 1].Value.ToLoverCase();
                CaseStage newStage = null;
                switch (stageType)
                {
                    case "none":
                        break;
                    case "pool":
                        newStage = CreatePollStage(worksheet, i);
                        break;
                    case "end":
                        newStage = CreateEndStage(worksheet, i);
                        break;
                    case "message":
                        newStage = CreateMessageStage(worksheet, i);
                        break;
                    default:
                        throw new ArgumentException($"No Such parameter. Row {i}, column 1");
                }
                newStage.Number = worksheet.Cells[i, 2];
                newStage.ModuleNumber = worksheet.Cells[i, 3];
                newStage.NextStage = worksheet.Cells[i, 4];
                newStage.TextBefore = worksheet.Cells[i, 5];
                newStage.AdditionalInfoType = Enum.Parse(typeof(AdditionalInfo), worksheet.Cells[i, 6]);
                if(newStage.AdditionalInfoType != AdditionalInfo.None)
                {
                    newStage.NamesAdditionalFiles = worksheet.Cells[i, 7].ToString().Split(';').ToList();
                }
                stageListJObject[$"{stageType}-{i}"] = JObject.FromObject(newStage);
            }
            return stageListJObject.ToString();
        }

        private static CaseStageMessage CreateMessageStage(Worksheet worksheet, int lineNumber)
        {
            CaseStageMessage newStage = new CaseStageMessage();
            newStage.MessageTypeAnswer = Enum.Parse(typeof(MessageType), worksheet.Cells[lineNumber, 8]);
            newStage.Rate = worksheet.Cells[lineNumber, 9];
            return newStage;
        }

        private static CaseStageEndModule CreateEndStage(Worksheet worksheet, int i)
        {
            CaseStageEndModule newStage = new CaseStageEndModule();
            newStage.IsEndOfCase = worksheet.Cells[i, 8] == "false" ? false : true;
            newStage.Texts = worksheet.Cells[i, 9].ToString().Split(';').ToList();
            newStage.Rates = worksheet.Cells[i, 10].ToString().Split(';'); // Доделать
            return newStage;
        }

        private static CaseStagePoll CreatePollStage(Worksheet worksheet, int i)
        {
            CaseStagePoll newStage = new CaseStagePoll();
            
            newStage.ConditionalMove = worksheet.Cells[i, 8] == "false" ? false : true;
            // Переход на следующий стейдж зависит от ответа или нет?
            // Надо прописать логику перехода какую то если тру, а вот какую логику...

            newStage.ManyAnswers = worksheet.Cells[i, 9] == "false" ? false : true;
            newStage.Options = worksheet.Cells[i, 10].ToString().Split(';').ToList();

            if (!newStage.ManyAnswers)
                newStage.MovingNumbers = worksheet.Cells[i, 11];



            return newStage;
        }

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
