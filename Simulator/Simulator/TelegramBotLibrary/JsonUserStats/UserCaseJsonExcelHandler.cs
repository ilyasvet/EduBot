using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Simulator.Services;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using Simulator.Case;
using Simulator.Models;
using System;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary.JsonUserStats
{
    public static class UserCaseJsonExcelHandler
    {
        private static Dictionary<int, int> CountTasks;
        private static Dictionary<int, List<int>> StageNumbersModule;

        static UserCaseJsonExcelHandler()
        {
            // Номер этапа - количество заданий
            CountTasks = StagesControl.GetTaskCountDictionary();
            
            // Номер этапа - номера заданий
            StageNumbersModule = new Dictionary<int, List<int>>();
            foreach (var module in CountTasks)
            {
                StageNumbersModule.Add(module.Key, StagesControl.GetStageNumbers(module.Key));
            }
        }
        public static async Task CreateAndEditExcelFile(string path, string statsDirectory)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Interactive = false;
            excelApp.Visible = false;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                workbook = excelApp.Workbooks.Add();
                worksheet = (Worksheet)workbook.ActiveSheet;
                worksheet.Name = "Statistics";
                worksheet.Cells.WrapText = true;
                worksheet.Columns.ColumnWidth = 15;
                CreateExcelTitle(worksheet);

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

                int line = 4;
                foreach (string statsFileName in Directory.GetFiles(statsDirectory, "*.json"))
                {
                    await ParseUserStats(statsFileName, worksheet, line);
                    workbook.Save();
                    line++;
                }
                workbook.SaveAs(path);
            }
            finally
            {
                workbook.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL");
            }
        }

        public static async Task ParseUserStats(string statsFileName, Excel.Worksheet worksheet, int line)
        {
            long userId = long.Parse(statsFileName.Substring(statsFileName.LastIndexOf('\\')+1).Split('.')[0]);
            User user = UserTableCommand.GetUserById(userId);

            string nameSurname = user.Surname + " " + user.Name;
            string group = user.GroupNumber;

            try // Может и не быть
            {
                DateTime startCaseTime = UserCaseTableCommand.GetStartCaseTime(userId);
                worksheet.Cells[line, 4] = startCaseTime.ToString();
                DateTime endCaseTime = UserCaseTableCommand.GetEndCaseTime(userId);
                worksheet.Cells[line, 5] = endCaseTime.ToString();
            } catch { }

            int hp = UserCaseTableCommand.GetHealthPoints(userId);
            int attemptsUsed = hp == 0 ? 2 : hp == 1 ? 1 : 0;

            worksheet.Cells[line, 1] = nameSurname;
            worksheet.Cells[line, 2] = userId.ToString();
            worksheet.Cells[line, 3] = group;
            worksheet.Cells[line, 6] = attemptsUsed.ToString();
            

            double sumRateAllFirst = 0;
            double sumRateAllSecond = 0;

            string json = string.Empty;
            while (true)
            {
                try
                {
                    using (var fs = new FileStream(statsFileName, FileMode.Open))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            json = await sr.ReadToEndAsync();
                        }
                    }
                }
                catch (IOException)
                {
                    await Task.Delay(300);
                    continue;
                }
                break;
            }

            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

            int startColumn = 9;

            // string key = $"{moduleNumber}-{stageNumber}-{attemptNo}"
            foreach (var module in StageNumbersModule) // Проходимся по этапам
            {
                TimeSpan sumTimeFirst = default;
                TimeSpan sumTimeSecond = default;

                double sumRateFirst = 0;
                double sumRateSecond = 0;

                int curCount = 0;
                int countStages = module.Value.Count;
                foreach (var stage in module.Value)
                {
                    TimeSpan timeFirst = default;
                    TimeSpan timeSecond = default;
                    try
                    {
                        timeFirst = (TimeSpan)jsonObject[$"time-{module.Key}-{stage}-1"];
                        sumTimeFirst += timeFirst;
                    }
                    catch { }
                    try
                    {
                        timeSecond = (TimeSpan)jsonObject[$"time-{module.Key}-{stage}-2"];
                        sumTimeSecond += timeSecond;
                    } catch { }

                    if (timeFirst != default && timeSecond != default)
                    {
                        double averageTime = (timeFirst.TotalMinutes + timeSecond.TotalMinutes) / 2;
                        worksheet.Cells[line, startColumn + 2 + curCount] = averageTime.ToString();
                    }
                    else if(timeSecond != default)
                    {
                        worksheet.Cells[line, startColumn + 2 + curCount] = timeSecond.TotalMinutes.ToString();
                    }
                    else if(timeFirst != default)
                    {
                        worksheet.Cells[line, startColumn + 2 + curCount] = timeFirst.TotalMinutes.ToString();
                    }

                    try
                    {
                        double rateFirst = (double)jsonObject[$"rate-{module.Key}-{stage}-1"];
                        worksheet.Cells[line, startColumn + 2 + countStages * 1 + curCount] = rateFirst;
                        sumRateFirst += rateFirst;
                    }
                    catch { }
                    try
                    {
                        double rateSecond = (double)jsonObject[$"rate-{module.Key}-{stage}-2"];
                        worksheet.Cells[line, startColumn + 3 + countStages * 2 + curCount] = rateSecond;
                        sumRateSecond += rateSecond;
                    } catch { }

                    // countStages * n, n - номер секции
                    // startColumn + m, m - количество колонок с суммами до текущей

                    try
                    {
                        var answersFirst = jsonObject[$"answers-{module.Key}-{stage}-1"];
                        worksheet.Cells[line, startColumn + 4 + countStages * 3 + curCount] = answersFirst.ToString();
                    } catch { }
                    try
                    {
                        var answersSecond = jsonObject[$"answers-{module.Key}-{stage}-2"];
                        worksheet.Cells[line, startColumn + 4 + countStages * 4 + curCount] = answersSecond.ToString();
                    } catch { }


                    curCount++;
                }
                worksheet.Cells[line, startColumn] = sumTimeFirst.ToString();
                worksheet.Cells[line, startColumn + 1] = sumTimeSecond.ToString();

                worksheet.Cells[line, startColumn + 2 + countStages * 2] = sumRateFirst;
                worksheet.Cells[line, startColumn + 3 + countStages * 3] = sumRateSecond;

                startColumn += 4 + countStages * 5;

                sumRateAllFirst += sumRateFirst;
                sumRateAllSecond += sumRateSecond;
            }
            worksheet.Cells[line, 7] = sumRateAllFirst;
            worksheet.Cells[line, 8] = sumRateAllSecond;
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

            // Заполняем каждый этап
            foreach (var stagesCount in CountTasks)
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
                    worksheet.Cells[
                        2, startFromToMerge
                        + stagesCount.Value
                        + dictStatisticsStruct[i].Item2 - 1
                        ]
                ];
                    range.Merge();
                    worksheet.Cells[2, startFromToMerge] = dictStatisticsStruct[i].Item1;
                    
                    for (int j = 0; j < stagesCount.Value; j++)
                    {
                        worksheet.Cells[3, startFromToMerge] = $"П{StageNumbersModule[stagesCount.Key][j]}";
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
