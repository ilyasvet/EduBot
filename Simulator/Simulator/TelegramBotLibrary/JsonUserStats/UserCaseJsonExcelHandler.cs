using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Simulator.Services;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.TelegramBotLibrary.JsonUserStats
{
    public static class UserCaseJsonExcelHandler
    {
        public static void CreateAndEditExcelFile(string path)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            try
            {
                Excel.Worksheet worksheet = (Worksheet)workbook.ActiveSheet;
                worksheet.Name = "Statistics";

                CreateExcelTitle(worksheet);

                

                // ???
                workbook.SaveAs("test");
            }
            catch
            {
                throw;
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL");
            }
        }

        public static void ParseUserStats(long userId)
        {
            string fileName = $"{userId}.json";

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

                int summPtsFirstAttempt = 0;
                int summPtsSecondAttempt = 0;

                /*bool foundKey = false;
                foreach (var keyValuePair in jsonObject)
                {
                    if (keyValuePair.Key == keyToSeatchFor)
                    {
                        // нашли ключ - модифицируем старое value. Сепаратор - это _|_. Хехе)0)0)))0)
                        string newValue = GetValueForJsonObject(keyValuePair.Value.ToObject<string>(), value, attemptNo);
                        jsonObject.Remove(keyValuePair.Key);
                        jsonObject.Add(keyValuePair.Key, newValue);
                        foundKey = true;
                        break;
                    }
                }*/
            }
            else
            {
                throw new FileNotFoundException($"Error parsing Json stats. File {fileName} not found");
            }
        }

        private static void CreateExcelTitle(Excel.Worksheet worksheet)
        {
            // ФИО
            Excel.Range range = worksheet.Range["A1:A3"];
            range.Merge();
            worksheet.Cells[1, 1] = "ФИО";
            // Telegram ID
            range = worksheet.Range["B1:B3"];
            range.Merge();
            worksheet.Cells[1, 2] = "Telegram ID";
            // Группа
            range = worksheet.Range["C1:C3"];
            range.Merge();
            worksheet.Cells[1, 3] = "Группа";
            // Дата первого запуска бота
            range = worksheet.Range["D1:D3"];
            range.Merge();
            worksheet.Cells[1, 4] = "Время первого запуска бота";
            // Дата последнего выполненного задания
            range = worksheet.Range["E1:E3"];
            range.Merge();
            worksheet.Cells[1, 5] = "Время последнего выполненного задания";
            // Количество использованных попыток
            range = worksheet.Range["F1:F3"];
            range.Merge();
            worksheet.Cells[1, 6] = "Кол-во использованных попыток";
            // Баллы, набранные в 1 попытке
            range = worksheet.Range["G1:G3"];
            range.Merge();
            worksheet.Cells[1, 7] = "Баллы за 1 попытку";
            // Баллы, набранные в 2 попытке
            range = worksheet.Range["E1:E3"];
            range.Merge();
            worksheet.Cells[1, 8] = "Баллы за 2 попытку";
            // Время прохождения за 1 попытку
            range = worksheet.Range["I2:I3"];
            range.Merge();
            worksheet.Cells[2, 9] = "Время прохождения 1 попытки";
            // Время прохождения за 1 попытку
            range = worksheet.Range["J2:J3"];
            range.Merge();
            worksheet.Cells[2, 10] = "Время прохождения 2 попытки";
        }

        private static void CountTasksInEachStage(string courseJsonName)
        {
            string fileName = $"{courseJsonName}.json";

            string json = File.ReadAllText(fileName);
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

            // создаем словарь для хранения кол-ва вопросов в каждом кейсе
            var countTasks = new Dictionary<int, int>();

            // считаем кол-во вопросов в каждой стадии кейса
            if (File.Exists(fileName))
            {
                foreach (var stage in jsonObject)
                {
                    // сохраняем записи текущей стадии во временный лист
                    var tasksArray = stage.Value.ToObject<List<object>>();
                    var count = tasksArray.Count;

                    countTasks.Add(int.Parse(stage.Key), count);
                }
            }
            else
            {
                throw new FileNotFoundException($"Json course file does not exist. File {fileName} not found");
            }
        }
    }
}
