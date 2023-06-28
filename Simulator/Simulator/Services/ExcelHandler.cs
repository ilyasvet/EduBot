using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Simulator.Models;
using Simulator.TelegramBotLibrary;
using System;
using System.IO;
using Telegram.Bot.Types.Enums;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulator.Services
{
    internal static class ExcelHandler
    {
        public static async Task<string> CreateCaseAsync(string path)
        {
            Excel.Application excelApp = new();
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
                    using (StreamWriter sw = new(fs))
                    {
                        await sw.WriteAsync(await AddCaseStageIterative(worksheet, urRows.Count));
                    }
                }
                return "case.json";

            }
            finally
            {
                workbook.Close(); //освобождаем неуправляемые ресурсы
                workbooks.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL"); //и завершаем процесс, чтобы он не висел
            }
        }

        private static async Task<string> AddCaseStageIterative(Worksheet worksheet, int count)
        {
            JObject stageListJObject = new();
            await Task.Run(() =>
            {
                for (int i = 2; i <= count; i++)
                {
                    string stageType;
                    stageType = worksheet.Cells[i, 1].Value.ToLower();
                    CaseStage newStage = null;
                    switch (stageType)
                    {
                        case "none":
                            newStage = new CaseStageNone();
                            break;
                        case "poll":
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
                    newStage.Number = int.Parse(worksheet.Cells[i, 2].Value.ToString());
                    newStage.ModuleNumber = int.Parse(worksheet.Cells[i, 3].Value.ToString());
                    newStage.NextStage = int.Parse(worksheet.Cells[i, 4].Value.ToString());
                    newStage.TextBefore = worksheet.Cells[i, 5].Value.ToString();
                    newStage.AdditionalInfoType = Enum.Parse(typeof(AdditionalInfo), worksheet.Cells[i, 6].Value.ToString());
                    if (newStage.AdditionalInfoType != AdditionalInfo.None)
                    {
                        newStage.NamesAdditionalFiles = new List<string>(worksheet.Cells[i, 7].Value.ToString().Split(';'));
                    }
                    stageListJObject[$"{stageType}-{i}"] = JObject.FromObject(newStage);
                }
            });
            return stageListJObject.ToString();
        }

        private static CaseStageMessage CreateMessageStage(Worksheet worksheet, int lineNumber)
        {
            CaseStageMessage newStage = new()
            {
                MessageTypeAnswer = Enum.Parse(typeof(MessageType), worksheet.Cells[lineNumber, 8].Value.ToString()),
                Rate = double.Parse(worksheet.Cells[lineNumber, 9].Value.ToString())
            };
            return newStage;
        }

        private static CaseStageEndModule CreateEndStage(Worksheet worksheet, int i)
        {
            CaseStageEndModule newStage = new()
            {
                IsEndOfCase = worksheet.Cells[i, 8].Value.ToString() == "false" ? false : true,
                Texts = new List<string>(worksheet.Cells[i, 9].Value.
                ToString().
                Split(';'))
            };

            List<string> stringsRate = new List<string>(worksheet.Cells[i, 10].Value.
                ToString().
                Split(';'));
            foreach (string s in stringsRate)
            {
                double.TryParse(s, out double rate);
                newStage.Rates.Add(rate);
            }
            return newStage;
        }

        private static CaseStagePoll CreatePollStage(Worksheet worksheet, int i)
        {
            CaseStagePoll newStage = new()
            {
                ConditionalMove = worksheet.Cells[i, 8].Value.ToString() == "false" ? false : true,
                // Переход на следующий стейдж зависит от ответа или нет?
                // Надо прописать логику перехода какую то если тру, а вот какую логику...

                ManyAnswers = worksheet.Cells[i, 9].Value.ToString() == "false" ? false : true,
                Options = new List<string>(worksheet
                .Cells[i, 10].Value
                .ToString()
                .Split(';'))
            };


            // MovingNumbers
            if (!newStage.ManyAnswers && newStage.ConditionalMove)
            {
                // делаем лист стрингов из исходной ячейки эксельки
                var tmpMovingNumbersString = worksheet
                    .Cells[i, 11].Value
                    .ToString();
                List<string> tmpListMovingNumbers = new List<string>(tmpMovingNumbersString
                    .Split(';'));

                // дальше работаем с листом и забиваем его данные в newStage.MovingNumbers
                foreach (string item in tmpListMovingNumbers)
                {
                    // было, например = ' 1-5 '. parts будет = ' 1 ' и ' 5 '
                    string[] parts = item.Split('-');
                    if (int.TryParse(parts[0], out int key) && int.TryParse(parts[1], out int value))
                    {
                        newStage.MovingNumbers.Add(key, value);
                    }
                }
            }


            // PossibleRate
            dynamic tmpPossibleRateString = worksheet
                .Cells[i, 12].Value
                .ToString();
            List<string> tmpListPossibleRate = new List<string>(tmpPossibleRateString
                .Split(';'));
            foreach (string item in tmpListPossibleRate)
            {
                string[] parts = item.Split('-');
                if (int.TryParse(parts[0], out int key) && int.TryParse(parts[1], out int value))
                {
                    newStage.PossibleRate.Add(key, value);
                }
            }

            // WatchNonAnswers
            newStage.WatchNonAnswer = worksheet.Cells[i, 13].Value.ToString() == "false" ? false : true;

            // NonAnswers
            if (newStage.WatchNonAnswer)
            {
                dynamic tmpNonAnswersString = worksheet
                    .Cells[i, 14].Value
                    .ToString();
                List<string> tmpListNonAnswers = new List<string>(tmpNonAnswersString
                    .Split(';'));
                foreach (string item in tmpListNonAnswers)
                {
                    string[] parts = item.Split('-');
                    if (int.TryParse(parts[0], out int key) && int.TryParse(parts[1], out int value))
                    {
                        newStage.NonAnswers.Add(key, value);
                    }
                }
            }

            // Limit
            newStage.Limit = int.Parse(worksheet.Cells[i, 15].Value.ToString());

            // Fine
            newStage.Fine = double.Parse(worksheet.Cells[i, 16].Value.ToString());

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
