using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Simulator.Services;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using Simulator.Case;

namespace Simulator.TelegramBotLibrary.JsonUserStats
{
    public static class UserCaseJsonExcelHandler
    {
        public static void CreateAndEditExcelFile(string path, bool created, string statsDirectory)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                try
                {
                    if (!created)
                    {
                        workbook = excelApp.Workbooks.Add();
                        worksheet = (Worksheet)workbook.ActiveSheet;
                        worksheet.Name = "Statistics";
                        worksheet.Cells.WrapText = true;
                        worksheet.Columns.ColumnWidth = 15;
                        CreateExcelTitle(worksheet);
                        workbook.SaveAs(path);
                        workbook.Close();
                    }
                }
                catch
                {
                    File.Delete(path);
                    throw;
                }

                workbook = excelApp.Workbooks.Open(path);
                worksheet = workbook.Worksheets[1];

                // Удаляем старые записи
                int lastRow = worksheet.UsedRange.Rows.Count;
                int lastColumn = worksheet.UsedRange.Columns.Count;
                if(lastRow >= 4)
                {
                    Range workRange = worksheet.Range[worksheet.Cells[4, 1],
                    worksheet.Cells[lastRow, lastColumn]];
                    workRange.Delete();
                    workbook.Save();
                }
                

                foreach (string statsFileName in Directory.GetFiles(statsDirectory, "*.json"))
                {
                    ParseUserStats(statsFileName, worksheet);
                    workbook.Save();
                }
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL");
            }
        }

        public static void ParseUserStats(string statsFileName, Excel.Worksheet worksheet)
        {
            string json = File.ReadAllText(statsFileName);
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

            int summPtsFirstAttempt = 0;
            int summPtsSecondAttempt = 0;

            /*bool foundKey = false;
            foreach (var stagesCount in jsonObject)
            {
                if (stagesCount.Key == keyToSeatchFor)
                {
                    // нашли ключ - модифицируем старое value. Сепаратор - это _|_. Хехе)0)0)))0)
                    string newValue = GetValueForJsonObject(stagesCount.Value.ToObject<string>(), value, attemptNo);
                    jsonObject.Remove(stagesCount.Key);
                    jsonObject.Add(stagesCount.Key, newValue);
                    foundKey = true;
                    break;
                }
            }*/
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
            range = worksheet.Range["H1:H3"];
            range.Merge();
            worksheet.Cells[1, 8] = "Баллы за 2 попытку";


            // введем стартовую переменную по левому краю эксель таблицы, начиная от которой будем
            // объединять ячейки таблицы (последний столбец, который во всех таблицах будет одинаковый - 8)
            int startFromToMerge = 9;

            const int sectionsCount = 5;
            const int cellsTimeStages = 2;
            const int cellsSummPtsStages = 2;

            // Номер этапа - количество заданий
            Dictionary<int, int> countTasks = StagesControl.GetTaskCountDictionary();

            // Заполняем каждый этап
            foreach (var stagesCount in countTasks)
            {
                // высчитываем ширину текущего этапа по клеточкам
                int rangeToMergeCells = cellsTimeStages + cellsSummPtsStages + (stagesCount.Value * 5);

                // смотрим конец объединенной ячейки
                int endToMerge = startFromToMerge + rangeToMergeCells - 1;

                // длинная ячейка с номером этапа
                range = worksheet.Range[
                    worksheet.Cells[1, startFromToMerge],
                    worksheet.Cells[1, endToMerge]
                    ];
                range.Merge();
                worksheet.Cells[1, startFromToMerge] = $"Этап {stagesCount.Key}";


                // в каждом этапе есть 7 больших блоков текста. Формируем их
                for (int i = 1; i <= 2; i++)
                {
                    range = worksheet.Range[
                    worksheet.Cells[2, startFromToMerge],
                    worksheet.Cells[3, startFromToMerge]
                ];
                    range.Merge();
                    worksheet.Cells[2, startFromToMerge] = $"Время прохождения {i} попытки";
                    startFromToMerge++;

                }

                // Словарь строка - наименование секции.
                // Количество столбцов секции - количество вопросов в этапе
                // int - дополнительные столбцы
                var dictStatisticsStruct = new List<(string, int)>
                {
                    ("Среднее время за все попытки", 0),
                    ("Баллы, набранные за 1 попытку", 1),
                    ("Баллы, набранные за 2 попытку", 1),
                    ("Ответы, выбранные в ходе попытки 1", 0),
                    ("Ответы, выбранные в ходе попытки 2", 0)
                };
                for (int i = 0; i < sectionsCount; i++)
                {
                    range = worksheet.Range[
                    worksheet.Cells[2, startFromToMerge],
                    worksheet.Cells[2, startFromToMerge
                        + stagesCount.Value
                        + dictStatisticsStruct[i].Item2 - 1]
                ];
                    range.Merge();
                    worksheet.Cells[2, startFromToMerge] = dictStatisticsStruct[i].Item1;
                    List<int> stageNumbersCurrentModule = StagesControl.GetStageNumbers(stagesCount.Key);
                    for (int j = 0; j < stagesCount.Value; j++)
                    {
                        worksheet.Cells[3, startFromToMerge] = $"П{stageNumbersCurrentModule[j]}";
                        startFromToMerge++;
                    }
                    for (int j = 0; j < dictStatisticsStruct[i].Item2; j++)
                    {
                        worksheet.Cells[3, startFromToMerge] = "Итог";
                        startFromToMerge++;
                    }
                }
            }
        }
    }  
}
