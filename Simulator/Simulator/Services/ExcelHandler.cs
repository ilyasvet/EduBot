using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Simulator.Models;
using System;
using System.IO;
using Telegram.Bot.Types.Enums;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simulator.BotControl;

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
                workbook = workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "\\" + path); //открываем файл excel
                Excel.Worksheet worksheet = workbook.Worksheets[1]; //берём 1 страницу (счёт с 1)

                // Получаем диапазон используемых на странице ячеек
                Excel.Range usedRange = worksheet.UsedRange;
                // Получаем строки в используемом диапазоне
                Excel.Range urRows = usedRange.Rows;
                // Получаем столбцы в используемом диапазоне
                Excel.Range urColums = usedRange.Columns;

                string caseFileName = ControlSystem.caseInfoFileName;

                using (Stream fs = new FileStream(caseFileName, FileMode.Create))
                {
                    using (StreamWriter sw = new(fs))
                    {
                        await sw.WriteAsync(await AddCaseStageIterative(worksheet, urRows.Count));
                    }
                }
                return caseFileName;

            }
            finally
            {
                //освобождаем неуправляемые ресурсы
                workbooks.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL"); //и завершаем процесс, чтобы он не висел
            }
        }

        private static async Task<string> AddCaseStageIterative(Worksheet worksheet, int count)
        {
            var stageList = new StageList();

            await Task.Run(() =>
            {
                // Общие свойства - свойства, которые есть у всех вопросов
                // Специализированные свойства - свойства, пренадлежащие к отдельному типу вопроса
                for (int i = 2; i <= count; i++)
                {
                    int j = 1; // Номер столбца
                    try
                    {   
                        string stageType; // Тип вопроса

                        // Находится в 1 столбце
                        stageType = worksheet.Cells[i, j].Value.ToLower();
                        j = 7; // Сначала заполняются специализированные свойства, они начинаются с 8 столбца

                        CaseStage newStage = null;
                        // Ссылка на новый объект вопроса

                        // j передаётся по ссылке
                        switch (stageType)
                        {
                            case "none":
                                newStage = new CaseStageNone();
                                stageList.StagesNone.Add(newStage as CaseStageNone);
                                break;
                            case "poll":
                                newStage = CreatePollStage(worksheet, i, ref j);
                                stageList.StagesPoll.Add(newStage as CaseStagePoll);
                                break;
                            case "end":
                                newStage = CreateEndStage(worksheet, i, ref j);
                                stageList.StagesEnd.Add(newStage as CaseStageEndModule);
                                break;
                            case "message":
                                newStage = CreateMessageStage(worksheet, i, ref j);
                                stageList.StagesMessage.Add(newStage as CaseStageMessage);
                                break;
                            default:
                                j = 1;
                                throw new ArgumentException($"No Such parameter");
                        }

                        j = 2; // Теперь заполняем общие свойства

                        // Номер вопроса
                        newStage.Number = int.Parse(worksheet.Cells[i, j].Value?.ToString());
                        j++;

                        // Номер модуля
                        newStage.ModuleNumber = int.Parse(worksheet.Cells[i, j].Value?.ToString());
                        j++;

                        // Номер следующего вопроса
                        newStage.NextStage = int.Parse(worksheet.Cells[i, j].Value?.ToString());
                        j++;

                        // Основной тест вопроса
                        newStage.TextBefore = worksheet.Cells[i, j].Value?.ToString();
                        j++;
                        if(stageType == "poll" && newStage.TextBefore.Length >= 300)
                        {
                            throw new ArgumentException("Poll question lenght must be no more 300 characters");
                        }


                        // Если дополнительная информация имеется, то последовательность имён файлов
                        List<string> additionalFiles = new List<string>(worksheet.Cells[i, j].Value?.ToString().Split(';') ?? 0);
                        foreach (string fileName in additionalFiles)
                        {
                            newStage.AddAdditionalFile(fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.Message}\nLine {i} column {j}");
                    }
                }
            });
            return JObject.FromObject(stageList).ToString();
        }

        private static CaseStageMessage CreateMessageStage(Worksheet worksheet, int lineNumber, ref int j)
        {
            CaseStageMessage newStage = new();

            // Тип ответа (видео, аудио). Обязательное поле
            newStage.MessageTypeAnswer = Enum.Parse(typeof(MessageType), worksheet.Cells[lineNumber, j].Value?.ToString());
            j++;
            
            // Количество баллов за ответ. Обязательное поле
            newStage.Rate = double.Parse(worksheet.Cells[lineNumber, j].Value?.ToString());
            return newStage;
        }

        private static CaseStageEndModule CreateEndStage(Worksheet worksheet, int i, ref int j)
        {
            CaseStageEndModule newStage = new();

            // Является ли концом курса. Обязательное поле
            string isEndOfCase = worksheet.Cells[i, j].Value?.ToString();
            switch (isEndOfCase.ToLower())
            {
                case "false":
                    newStage.IsEndOfCase = false;
                    break;
                case "true":
                    newStage.IsEndOfCase = true;
                    break;
                default:
                    throw new ArgumentException("No such parameter");
            }
            j++;

            // Градации по рейтингу. Обязательное поле
            List<string> stringsRate = new List<string>(worksheet.Cells[i, j].Value.ToString().
                Split(';'));

            foreach (string s in stringsRate)
            {
                double rate = double.Parse(s);
                newStage.Rates.Add(rate);
            }
            j++;

            // Количество текстов. Обязательное поле
            int countTexts = int.Parse(worksheet.Cells[i, j].Value?.ToString());
            j++;

            if (countTexts != stringsRate.Count + 3)
            {
                throw new ArgumentException("Texts count must be rate gradations + 3");
            }

            // Текста. Обязательное поле
            newStage.Texts = new List<string>();
            for (int k = 0; k < countTexts; k++)
            {
                string text = worksheet.Cells[i, j].Value.ToString();
                newStage.Texts.Add(text);
                j++;
            } 

            return newStage;
        }

        private static CaseStagePoll CreatePollStage(Worksheet worksheet, int i, ref int j)
        {
            CaseStagePoll newStage = new();
            
            // Налицие условного перехода между вопросами - 8. По умолчанию false
            string conditionalMove = worksheet.Cells[i, j].Value?.ToString() ?? string.Empty;
            newStage.ConditionalMove = conditionalMove.ToLower() == "true";
            j++;

            // Множественный ответ или нет - 9. По умолчанию false
            string manyAnswers = worksheet.Cells[i, j].Value?.ToString() ?? string.Empty;
            newStage.ManyAnswers = manyAnswers.ToLower() == "true";
            j++;

            if (newStage.ManyAnswers && newStage.ConditionalMove)
                throw new ArgumentException("Many answers with conditional move cannot be together");

            // Смотрим неотмеченные ответы или нет - 10. По умолчанию false
            string watchNonAnswers = worksheet.Cells[i, j].Value?.ToString() ?? string.Empty;
            newStage.WatchNonAnswer = watchNonAnswers.ToLower() == "true";
            j++;

            if (newStage.ConditionalMove && newStage.WatchNonAnswer)
                throw new ArgumentException("Conditional move with watching non-answers cannot be together");
            if (!newStage.ManyAnswers && newStage.WatchNonAnswer)
                throw new ArgumentException("Single answer with watching non-answers cannot be together");
            

            // Лимит по ответам и штраф за превышение - 11 и 12. По умолчанию 0
            if (newStage.ManyAnswers)
            {
                try
                {
                    newStage.Limit = int.Parse(worksheet.Cells[i, j].Value?.ToString());
                } catch {}
                j++;
                try
                {
                    newStage.Fine = double.Parse(worksheet.Cells[i, j].Value?.ToString());
                } catch {}
                j++;
            }
            else
            {
                j += 2;
            }

            // Количество вариантов ответов - 13. Обязательное поле
            int countOptions = int.Parse(worksheet.Cells[i, j].Value?.ToString());
            j++;

            // Ячейки: вариант ответа;баллы;баллы при отсутствии|адрес перехода
            newStage.Options = new List<string>();
            for(int k = 0; k < countOptions; k++)
            {
                string optionString = worksheet.Cells[i, j].Value.ToString();
                string[] optionProperties = optionString.Split(';');
                
                if (optionProperties[0].Length > 100)
                {
                    throw new ArgumentException("Option lenght must be no more 100 characters");
                }

                newStage.Options.Add(optionProperties[0]);

                double rate = double.Parse(optionProperties[1]);
                newStage.PossibleRate.Add(k, rate);   
                
                if (newStage.WatchNonAnswer)
                {
                    double nonRate = double.Parse(optionProperties[2]);
                    newStage.NonAnswers.Add(k, nonRate);
                }
                else if (newStage.ConditionalMove)
                {
                    int movePoint = int.Parse(optionProperties[2]);
                    newStage.MovingNumbers.Add(k, movePoint);
                }
                j++;
            }    

            return newStage;
        }

        public static async Task<int> AddUsersFromExcel(string path, string groupNumber)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            int count = 0;
            try
            {
                workbooks = excelApp.Workbooks; //это хранилище наших файлов, с которыми мы работаем
                workbook = workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "\\" + path); //открываем файл excel
                Excel.Worksheet worksheet = workbook.Worksheets[1]; //берём 1 страницу (счёт с 1)

                // Получаем диапазон используемых на странице ячеек
                Excel.Range usedRange = worksheet.UsedRange;
                // Получаем строки в используемом диапазоне
                Excel.Range urRows = usedRange.Rows;
                // Получаем столбцы в используемом диапазоне
                Excel.Range urColums = usedRange.Columns;

                if (urColums.Count != 3) throw new ArgumentException("В таблице должно быть только 3 столбца!");

                count = await AddUsersIterative(worksheet, urRows.Count, groupNumber); //построчно добавляем пользователей
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

        private static async Task<int> AddUsersIterative(Worksheet worksheet, int rowsCount, string groupNumber)
        {
            return await Task.Run(async () =>
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
                    if (await DataBaseControl.UserTableCommand.HasUser(userTelegramId))
                    {
                        continue; //Если пользователь уже есть, повторно не добавляем его
                    }
                    string userName = worksheet.Cells[i, 1].Value; //Берём имя из 1 столбца
                    string userSurname = worksheet.Cells[i, 2].Value; //Фамилию из 2 столбца

                    User user; //Создаём пользователя. И свойства проверяют входные данные
                    try
                    {
                        user = new User(userTelegramId, userName, userSurname);
                    }
                    catch (Exception ex) //При неправильных данных выдаётся исключение с номером строки с ошибкой
                    {
                        throw new ArgumentException(ex.Message + " Строка " + i);
                    }
                    user.GroupNumber = groupNumber; //Номер группы мы уже проверяли ранее

                    try
                    {
                        await DataBaseControl.UserTableCommand.AddUser(user, UserType.User);
                    }
                    catch
                    {
                        await DataBaseControl.UserTableCommand.DeleteUser(userTelegramId);
                        throw;
                    }

                    count++; //Увеличили счётчик добавленных пользователей
                }
                return count;
            });
        }
    }
}
