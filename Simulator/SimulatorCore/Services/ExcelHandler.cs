using Newtonsoft.Json.Linq;
using Simulator.Models;
using Telegram.Bot.Types.Enums;
using Simulator.BotControl;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;

namespace Simulator.Services
{
    internal static class ExcelHandler
    {
        public static async Task<string> MakeStatistics(string courseName, string groupNumber)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + 
                ControlSystem.caseDirectory + $"Statistics-{courseName}.xlsx";

            Excel.Application excelApp = new();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            try
            {
                workbooks = excelApp.Workbooks;
                workbook = workbooks.Add();

                await AddSheet(workbook, "BaseStats", $"Stats{courseName}Base", groupNumber);
                await AddSheet(workbook, "RateStats", $"Stats{courseName}Rate", groupNumber);
                await AddSheet(workbook, "TimeStats", $"Stats{courseName}Time", groupNumber);
                await AddSheet(workbook, "AnswersStats", $"Stats{courseName}Answers", groupNumber);

                Worksheet first = workbook.ActiveSheet;
                first.Delete();

                workbook.SaveAs(path);
                return path;
            }
            finally
            {
                //освобождаем неуправляемые ресурсы
                workbooks.Close();
                excelApp.Quit();
                ControlSystem.KillProcess("EXCEL"); //и завершаем процесс, чтобы он не висел
            }
        }

        private static async Task AddSheet(
            Workbook workbook, string sheetName, string tableName, string groupNumber)
        {
            Worksheet worksheet = workbook.ActiveSheet;
            worksheet.Cells.WrapText = true;
            worksheet.Columns.ColumnWidth = 20;
            worksheet.Name = sheetName;
            workbook.Sheets.Add(worksheet);

            List<string> header = await DataBaseControl.StatsBuilderCommand.GetColumnsName(tableName);
            int lineNumber = 1;
            for (int i = 1; i <= header.Count; i++)
            {
                worksheet.Cells[lineNumber, i] = header[i-1];
            }
            worksheet.Range[
                worksheet.Cells[1, 1],
                worksheet.Cells[1, header.Count]
                ].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
            lineNumber++;

            List<List<object>> statsData = await DataBaseControl.StatsBuilderCommand.GetAllTable(tableName, groupNumber);
            Dictionary<long, string> usersData = await DataBaseControl.StatsBuilderCommand.GetUsers(groupNumber);

            int columnNumber;

            foreach(var userLine in statsData)
            {
                columnNumber = 1;
                // Заменяем userID на его имя и фамилию
                userLine[0] = usersData[(int)userLine[0]];
                foreach(var userProperty in userLine)
                {
                    worksheet.Cells[lineNumber, columnNumber] = userProperty.ToString();
                    columnNumber++;
                }
                lineNumber++;
            }
            worksheet.Range[
                worksheet.Cells[2, 1],
                worksheet.Cells[statsData.Count + 1, 1]
                ].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);
        }

        public static async Task<string> CreateCaseAsync(string path)
		{
            path = AppDomain.CurrentDomain.BaseDirectory + "\\" + path;
			using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
			{
                WorkbookPart? workbookPart = document.WorkbookPart;
                Sheets? sheets = workbookPart?.Workbook.GetFirstChild<Sheets>();
                Sheet? mainSheet = sheets?.GetFirstChild<Sheet>();
                SheetData? sheetData = mainSheet?.GetFirstChild<SheetData>();

				string caseFileName = ControlSystem.caseInfoFileName;

				using (Stream fs = new FileStream(caseFileName, FileMode.Create))
				{
					using (StreamWriter sw = new(fs))
					{
						var stageList = new StageList();
						await MakeCaseHeader(sheetData, stageList);
						await AddCaseStageIterative(sheetData, stageList);
						await sw.WriteAsync(JObject.FromObject(stageList).ToString());
					}
				}
				return caseFileName;
			}
		}

        private static async Task MakeCaseHeader(SheetData? sheetData, StageList stageList)
        {
            await Task.Run(() =>
            {
                int lineNumber = 1;
                int columnNumber = 2;

                var sheetDataTable = sheetData?.ToList();

                stageList.CourseName = sheetDataTable[lineNumber].ToList()[columnNumber].InnerText;
                lineNumber++;

                string reCreateStats = sheetDataTable[lineNumber].ToList()[columnNumber].InnerText ?? string.Empty;
                stageList.ReCreateStats = reCreateStats.ToLower() == "true";
                lineNumber++;

                stageList.AttemptCount = int.Parse(sheetDataTable[lineNumber].ToList()[columnNumber].InnerText);
                lineNumber++;

                string extraAttempt = sheetDataTable[lineNumber].ToList()[columnNumber].InnerText ?? string.Empty;
                stageList.ExtraAttempt = extraAttempt.ToLower() == "true";
                lineNumber++;

                string deletePollAfterAnswer = sheetDataTable[lineNumber].ToList()[columnNumber].InnerText ?? string.Empty;
                stageList.DeletePollAfterAnswer = deletePollAfterAnswer.ToLower() == "true";
                lineNumber++;
            });
        }

        private static async Task AddCaseStageIterative(SheetData? sheetData, StageList stageList)
        {
            await Task.Run(() =>
            {
                // Общие свойства - свойства, которые есть у всех вопросов
                // Специализированные свойства - свойства, пренадлежащие к отдельному типу вопроса
                var sheetDataCaseOnly = sheetData.ToList().Skip(StageList.HEADER_PROPERTIES_COUNT+1);

				foreach (Row row in sheetDataCaseOnly)
                {
                    var rowValues = row.ToList();
                    int columnNumber = 0; // Номер столбца
                    try
                    {   
                        string stageType; // Тип вопроса

                        // Находится в 1 столбце
                        stageType = row.ToList()[columnNumber].InnerText.ToLower();
                        
                        // Сначала заполняются специализированные свойства, они начинаются с 8 столбца (7 индекс)
                        columnNumber = 7;

                        CaseStage newStage = null;
                        // Ссылка на новый объект вопроса

                        // columnNumber передаётся по ссылке
                        switch (stageType)
                        {
                            case "none":
                                newStage = new CaseStageNone();
                                stageList.StagesNone.Add(newStage as CaseStageNone);
                                break;
                            case "poll":
                                newStage = CreatePollStage(rowValues, ref columnNumber);
                                stageList.StagesPoll.Add(newStage as CaseStagePoll);
                                break;
                            case "end":
                                newStage = CreateEndStage(rowValues, ref columnNumber);
                                stageList.StagesEnd.Add(newStage as CaseStageEndModule);
                                break;
                            case "message":
                                newStage = CreateMessageStage(rowValues, ref columnNumber);
                                stageList.StagesMessage.Add(newStage as CaseStageMessage);
                                break;
                            default:
                                columnNumber = 0;
                                throw new ArgumentException($"No Such parameter");
                        }

                        columnNumber = 1; // Теперь заполняем общие свойства

                        // Номер вопроса
                        newStage.Number = int.Parse(rowValues[columnNumber].InnerText);
                        columnNumber++;

                        // Номер модуля
                        newStage.ModuleNumber = int.Parse(rowValues[columnNumber].InnerText);
                        columnNumber++;

                        // Номер следующего вопроса
                        newStage.NextStage = int.Parse(rowValues[columnNumber].InnerText);
                        columnNumber++;

                        // Основной тест вопроса
                        newStage.TextBefore = rowValues[columnNumber].InnerText;
                        columnNumber++;
                        if(stageType == "poll" && newStage.TextBefore.Length >= 300)
                        {
                            throw new ArgumentException("Poll question lenght must be no more 300 characters");
                        }


                        // Если дополнительная информация имеется, то последовательность имён файлов
                        List<string> additionalFiles = new List<string>();
                        string additionalFilesString = rowValues[columnNumber].InnerText;

						if (!string.IsNullOrEmpty(additionalFilesString))
                        {
                            additionalFiles = additionalFilesString.Split(';').ToList();
						}
                        foreach (string fileName in additionalFiles)
                        {
                            newStage.AddAdditionalFile(fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.Message}\nLine {row.RowIndex} column {columnNumber+1}");
                    }
                }
            });
        }

        private static CaseStageMessage CreateMessageStage(List<OpenXmlElement> rowValues, ref int j)
        {
            CaseStageMessage newStage = new();

            // Тип ответа (видео, аудио). Обязательное поле
            newStage.MessageTypeAnswer = (MessageType)Enum.Parse(typeof(MessageType), rowValues[j].InnerText);
            j++;
            
            // Количество баллов за ответ. Обязательное поле
            newStage.Rate = double.Parse(rowValues[j].InnerText);
            return newStage;
        }

        private static CaseStageEndModule CreateEndStage(List<OpenXmlElement> rowValues, ref int j)
        {
            CaseStageEndModule newStage = new();

            // Является ли концом курса. Обязательное поле
            string isEndOfCase = rowValues[j].InnerText ?? string.Empty;
            newStage.IsEndOfCase = isEndOfCase.ToLower() == "true";
            j++;

            // Градации по рейтингу. Обязательное поле
            List<string> stringsRate = new List<string>(rowValues[j].InnerText.
                Split(';'));

            foreach (string s in stringsRate)
            {
                double rate = double.Parse(s);
                newStage.Rates.Add(rate);
            }
            j++;

            // Количество текстов. Обязательное поле
            int countTexts = int.Parse(rowValues[j].InnerText);
            j++;

            if (countTexts != stringsRate.Count + 3)
            {
                throw new ArgumentException("Texts count must be rate gradations + 3");
            }

            // Текста. Обязательное поле
            newStage.Texts = new List<string>();
            for (int k = 0; k < countTexts; k++)
            {
                string text = rowValues[j].InnerText;
                newStage.Texts.Add(text);
                j++;
            } 

            return newStage;
        }

        private static CaseStagePoll CreatePollStage(List<OpenXmlElement> rowValues, ref int j)
        {
            CaseStagePoll newStage = new();
            
            // Налицие условного перехода между вопросами - 8. По умолчанию false
            string conditionalMove = rowValues[j].InnerText ?? string.Empty;
            newStage.ConditionalMove = conditionalMove.ToLower() == "true";
            j++;

            // Множественный ответ или нет - 9. По умолчанию false
            string manyAnswers = rowValues[j].InnerText ?? string.Empty;
            newStage.ManyAnswers = manyAnswers.ToLower() == "true";
            j++;

            if (newStage.ManyAnswers && newStage.ConditionalMove)
                throw new ArgumentException("Many answers with conditional move cannot be together");

            // Смотрим неотмеченные ответы или нет - 10. По умолчанию false
            string watchNonAnswers = rowValues[j].InnerText ?? string.Empty;
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
                    newStage.Limit = int.Parse(rowValues[j].InnerText);
                } catch {}
                j++;
                try
                {
                    newStage.Fine = double.Parse(rowValues[j].InnerText);
                } catch {}
                j++;
            }
            else
            {
                j += 2;
            }

            // Количество вариантов ответов - 13. Обязательное поле
            int countOptions = int.Parse(rowValues[j].InnerText);
            j++;

            // Ячейки: вариант ответа;баллы;баллы при отсутствии|адрес перехода
            newStage.Options = new List<string>();
            for(int k = 0; k < countOptions; k++)
            {
                string optionString = rowValues[j].InnerText;
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
                        await DataBaseControl.UserTableCommand.SetGroup(userTelegramId, user.GroupNumber);
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
