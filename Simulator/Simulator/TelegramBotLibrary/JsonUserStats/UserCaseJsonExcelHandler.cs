using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Simulator.Services;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

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
            

            // введем стартовую переменную по левому краю эксель таблицы, начиная от которой будем
            // объединять ячейки таблицы (последний столбец, который во всех таблицах будет одинаковый - 8)
            int startFromToMerge = 9;

            // этапы
            int cellsTimeStages = 2;
            int cellsSummPtsStages = 2;
            var countTasks = new Dictionary<int, int>();
            foreach (var keyValuePair in countTasks)
            {
                // высчитываем ширину текущего этапа по клеточкам
                int rangeToMergeCells = cellsTimeStages + cellsSummPtsStages + (keyValuePair.Value * 5);
                // смотрим конец объединенной ячейки
                int endToMerge = startFromToMerge + rangeToMergeCells;
                // длинная ячейка с номером этапа
                range = worksheet.Range[
                    worksheet.Cells[1, startFromToMerge],
                    worksheet.Cells[1, endToMerge]
                    ];
                range.Merge();
                worksheet.Cells[1, startFromToMerge] = $"Этап {keyValuePair.Key}";


                // в каждом этапе есть 7 больших блоков текста. Формируем их
                // Время прохождения за 1 попытку
                range = worksheet.Range[
                    worksheet.Cells[2, startFromToMerge],
                    worksheet.Cells[3, startFromToMerge]
                ];
                range.Merge();
                worksheet.Cells[2, startFromToMerge] = "Время прохождения 1 попытки";
                startFromToMerge += 1;
                // Время прохождения за 2 попытку
                range = worksheet.Range[
                    worksheet.Cells[2, startFromToMerge],
                    worksheet.Cells[3, startFromToMerge]
                ];
                range.Merge();
                worksheet.Cells[2, startFromToMerge] = "Время прохождения 2 попытки";
                startFromToMerge += 1;
                // Среднее время за все попытки
                range = worksheet.Range[
                    worksheet.Cells[2, startFromToMerge],
                    worksheet.Cells[2, startFromToMerge + keyValuePair.Key]
                ];
                range.Merge();
                worksheet.Cells[2, startFromToMerge] = "Среднее время за все попытки";
                startFromToMerge += 1 + keyValuePair.Key;
                // Баллы за 1 попытку
                range = worksheet.Range[
                    worksheet.Cells[2, startFromToMerge],
                    worksheet.Cells[2, startFromToMerge + cellsSummPtsStages / 2]
                ];
                range.Merge();
                worksheet.Cells[2, startFromToMerge] = "Баллы, набранные за 1 попытку";
                // надо дописать... Ну и проверить, верный ли алгоритм, без дебага нету смысла сейчас дописывать


                /*тута нада брать данные с жсон строки
                 * и вставлять их по столбцам*/


                // сдвигаем начало следующей клетки для объединения
                startFromToMerge += rangeToMergeCells + 1;
            }
        }

        private static Dictionary<int, int> CountTasksInEachStage(string courseJsonName)
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
            return countTasks;
        }
    }
}
