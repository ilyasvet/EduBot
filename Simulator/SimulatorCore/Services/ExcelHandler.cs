using Newtonsoft.Json.Linq;
using Simulator.Models;
using Telegram.Bot.Types.Enums;
using Simulator.BotControl;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using EduBotCore.Models.DbModels;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Simulator.Services
{
    internal static class ExcelHandler
    {
        public static async Task<string> MakeStatistics(string courseName, string groupNumber)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + 
                ControlSystem.caseDirectory + $"Statistics-{courseName}.xlsx";

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wp = document.AddWorkbookPart();
                wp.Workbook = new Workbook();

                WorksheetPart ws = wp.AddNewPart<WorksheetPart>();
				Sheets sheets = wp.Workbook.AppendChild(new Sheets());
                
				WorkbookStylesPart stylesheet = document.WorkbookPart.AddNewPart<WorkbookStylesPart>();
				Stylesheet workbookstylesheet = new Stylesheet();

                CellFormat cellformat = new CellFormat()
                {
                    Alignment = new Alignment() { WrapText = true},
                    
                };
                workbookstylesheet.Append(cellformat);

				stylesheet.Stylesheet = workbookstylesheet;
				stylesheet.Stylesheet.Save();

				await AddSheet(wp, sheets, "BaseStats", $"Stats{courseName}Base", groupNumber, 1);
                await AddSheet(wp, sheets, "RateStats", $"Stats{courseName}Rate", groupNumber, 2);
                await AddSheet(wp, sheets, "TimeStats", $"Stats{courseName}Time", groupNumber, 3);
                await AddSheet(wp, sheets, "AnswersStats", $"Stats{courseName}Answers", groupNumber, 4);

                wp.Workbook.Save();
                return path;
            }
        }

        private static async Task AddSheet(
            WorkbookPart wp, Sheets sheets, string sheetName, string tableName, string groupNumber, uint sheetId)
        {
			WorksheetPart ws = wp.AddNewPart<WorksheetPart>();
			Worksheet workSheet = new Worksheet();
			SheetData sheetData = new SheetData();

            List<string> header = await DataBaseControl.StatsBuilderCommand.GetColumnsName(tableName);
            Row headerRow = new Row();
            for (int i = 0; i < header.Count; i++)
            {
                headerRow.AppendChild(CreateCell(header[i]));
            }
            sheetData.AppendChild(headerRow);

            List<List<object>> statsData = await DataBaseControl.StatsBuilderCommand.GetAllTable(tableName, groupNumber);
            Dictionary<long, string> usersData = await DataBaseControl.StatsBuilderCommand.GetUsers(groupNumber);

            foreach(var userLine in statsData)
            {
				Row row = new Row();
				// Заменяем userID на его имя и фамилию
				userLine[0] = usersData[(int)userLine[0]];
                foreach(var userProperty in userLine)
                {
					row.AppendChild(CreateCell(userProperty));
                }
                sheetData.AppendChild(row);
            }

			Sheet sheet = new Sheet() { Id = wp.GetIdOfPart(ws), SheetId = sheetId, Name = sheetName };
			workSheet.AppendChild(sheetData);
			ws.Worksheet = workSheet;
			sheets.Append(sheet);

		}

        private static Cell CreateCell(object data)
        {
            Cell cell = new Cell();
            if(data is int ivalue)
            {
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(ivalue);
            }
            else if (data is double dvalue)
			{
				cell.DataType = CellValues.Number;
				cell.CellValue = new CellValue(dvalue);
			}
			else if (data is long lvalue)
			{
				cell.DataType = CellValues.Number;
				cell.CellValue = new CellValue((int)lvalue);
			}
			else if (data is string svalue)
			{
				cell.DataType = CellValues.String;
				cell.CellValue = new CellValue(svalue);
			}
			else if (data is DateTime dtvalue)
			{
				cell.DataType = CellValues.Date;
				cell.CellValue = new CellValue(dtvalue);
			}
            cell.StyleIndex = 0;
            return cell;
		}

        public static async Task<string> CreateCaseAsync(string path)
		{
            path = AppDomain.CurrentDomain.BaseDirectory + "\\" + path;
			using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
			{
                WorkbookPart? workbookPart = document.WorkbookPart;
                Sheets? sheets = workbookPart?.Workbook.GetFirstChild<Sheets>();
				Sheet? mainSheet = sheets?.GetFirstChild<Sheet>();
				Worksheet worksheet = (document.WorkbookPart.GetPartById(mainSheet.Id.Value) as WorksheetPart).Worksheet;
				SheetData? sheetData = worksheet?.GetFirstChild<SheetData>();

				string caseFileName = ControlSystem.caseInfoFileName;

				using (Stream fs = new FileStream(caseFileName, FileMode.Create))
				{
					using (StreamWriter sw = new(fs))
					{
						var stageList = new StageList();
						await MakeCaseHeader(workbookPart, sheetData, stageList);
						await AddCaseStageIterative(workbookPart, sheetData, stageList);
						await sw.WriteAsync(JObject.FromObject(stageList).ToString());
					}
				}
				return caseFileName;
			}
		}

        private static async Task MakeCaseHeader(WorkbookPart? workbookPart, SheetData? sheetData, StageList stageList)
        {
            await Task.Run(() =>
            {
                int lineNumber = 1;
                int columnNumber = 2;

                var sheetDataTable = sheetData?.ToList();
                
				stageList.CourseName = GetSharedStringItemById(workbookPart, sheetDataTable[lineNumber-1].ToList()[columnNumber-1].InnerText);
                lineNumber++;

				string reCreateStats = GetSharedStringItemById(workbookPart, sheetDataTable[lineNumber-1].ToList()[columnNumber-1].InnerText) ?? string.Empty;
                stageList.ReCreateStats = reCreateStats.ToLower() == "true";
                lineNumber++;

                stageList.AttemptCount = int.Parse(sheetDataTable[lineNumber - 1].ToList()[columnNumber-1].InnerText);
                lineNumber++;

                string extraAttempt = GetSharedStringItemById(workbookPart, sheetDataTable[lineNumber - 1].ToList()[columnNumber - 1].InnerText) ?? string.Empty;
                stageList.ExtraAttempt = extraAttempt.ToLower() == "true";
                lineNumber++;

                string deletePollAfterAnswer = GetSharedStringItemById(workbookPart, sheetDataTable[lineNumber - 1].ToList()[columnNumber - 1].InnerText) ?? string.Empty;
                stageList.DeletePollAfterAnswer = deletePollAfterAnswer.ToLower() == "true";
                lineNumber++;
            });
        }

        private static async Task AddCaseStageIterative(WorkbookPart? workbookPart, SheetData? sheetData, StageList stageList)
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
                        stageType = GetSharedStringItemById(workbookPart, row.ToList()[columnNumber].InnerText).ToLower();
                        
                        // Сначала заполняются специализированные свойства, они начинаются с 7 столбца (6 индекс)
                        columnNumber = 6;

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
                                newStage = CreatePollStage(workbookPart, rowValues, ref columnNumber);
                                stageList.StagesPoll.Add(newStage as CaseStagePoll);
                                break;
                            case "end":
                                newStage = CreateEndStage(workbookPart, rowValues, ref columnNumber);
                                stageList.StagesEnd.Add(newStage as CaseStageEndModule);
                                break;
                            case "message":
                                newStage = CreateMessageStage(workbookPart, rowValues, ref columnNumber);
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
                        newStage.TextBefore = GetSharedStringItemById(workbookPart, rowValues[columnNumber].InnerText);
                        columnNumber++;
                        if(stageType == "poll" && newStage.TextBefore.Length >= 300)
                        {
                            throw new ArgumentException("Poll question lenght must be no more 300 characters");
                        }


                        // Если дополнительная информация имеется, то последовательность имён файлов
                        List<string> additionalFiles = new List<string>();
                        string additionalFilesString = GetSharedStringItemById(workbookPart, rowValues[columnNumber].InnerText);

						if (!additionalFilesString.Equals("null"))
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

        private static CaseStageMessage CreateMessageStage(WorkbookPart? workbookPart, List<OpenXmlElement> rowValues, ref int j)
        {
            CaseStageMessage newStage = new();

            // Тип ответа (видео, аудио). Обязательное поле
            newStage.MessageTypeAnswer = (MessageType)Enum.Parse(typeof(MessageType),
                GetSharedStringItemById(workbookPart, rowValues[j].InnerText));
            j++;
            
            // Количество баллов за ответ. Обязательное поле
            newStage.Rate = double.Parse(rowValues[j].InnerText);
            return newStage;
        }

        private static CaseStageEndModule CreateEndStage(WorkbookPart? workbookPart, List<OpenXmlElement> rowValues, ref int j)
        {
            CaseStageEndModule newStage = new();

            // Является ли концом курса. Обязательное поле
            string isEndOfCase = GetSharedStringItemById(workbookPart, rowValues[j].InnerText) ?? string.Empty;
            newStage.IsEndOfCase = isEndOfCase.ToLower() == "true";
            j++;

            // Градации по рейтингу. Обязательное поле
            List<string> stringsRate = new List<string>(GetSharedStringItemById(workbookPart, rowValues[j].InnerText).
                Split(';'));
            stringsRate.Remove("");

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
                string text = GetSharedStringItemById(workbookPart, rowValues[j].InnerText);
                newStage.Texts.Add(text);
                j++;
            } 

            return newStage;
        }

        private static CaseStagePoll CreatePollStage(WorkbookPart? workbookPart, List<OpenXmlElement> rowValues, ref int j)
        {
            CaseStagePoll newStage = new();
            
            // Налицие условного перехода между вопросами - 8. По умолчанию false
            string conditionalMove = GetSharedStringItemById(workbookPart, rowValues[j].InnerText) ?? string.Empty;
            newStage.ConditionalMove = conditionalMove.ToLower() == "true";
            j++;

            // Множественный ответ или нет - 9. По умолчанию false
            string manyAnswers = GetSharedStringItemById(workbookPart, rowValues[j].InnerText) ?? string.Empty;
            newStage.ManyAnswers = manyAnswers.ToLower() == "true";
            j++;

            if (newStage.ManyAnswers && newStage.ConditionalMove)
                throw new ArgumentException("Many answers with conditional move cannot be together");

            // Смотрим неотмеченные ответы или нет - 10. По умолчанию false
            string watchNonAnswers = GetSharedStringItemById(workbookPart, rowValues[j].InnerText) ?? string.Empty;
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
                string optionString = GetSharedStringItemById(workbookPart, rowValues[j].InnerText);
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
            int count = 0;
			path = AppDomain.CurrentDomain.BaseDirectory + "\\" + path;
			using (SpreadsheetDocument document = SpreadsheetDocument.Open(path, false))
			{
				WorkbookPart? workbookPart = document.WorkbookPart;
				Sheets? sheets = workbookPart?.Workbook.GetFirstChild<Sheets>();
				Sheet? mainSheet = sheets?.GetFirstChild<Sheet>();
				Worksheet worksheet = (document.WorkbookPart.GetPartById(mainSheet.Id.Value) as WorksheetPart).Worksheet;
				SheetData? sheetData = worksheet?.GetFirstChild<SheetData>();

                count = await AddUsersIterative(workbookPart, sheetData, groupNumber); //построчно добавляем пользователей
            }
            return count;
        }

        private static async Task<int> AddUsersIterative(WorkbookPart? workbookPart, SheetData? sheetData, string groupNumber)
        {
            return await Task.Run(async () =>
            {
                int count = 0;

                foreach(Row row in sheetData)
                {
                    var rowValues = row.ToList();
                    long userTelegramId;
                    try
                    {
                        userTelegramId = long.Parse(rowValues[2].InnerText);
                    }
                    catch
                    {
                        throw new ArgumentException("TelegramId должен быть целым числом. Строка " + row.RowIndex);
                    }
                    if (await DataBaseControl.GetEntity<User>(userTelegramId) != null)
                    {
                        continue; //Если пользователь уже есть, повторно не добавляем его
                    }
					
					string userName = GetSharedStringItemById(workbookPart, rowValues[0].InnerText); //Берём имя из 1 столбца
                    string userSurname = GetSharedStringItemById(workbookPart, rowValues[1].InnerText); //Фамилию из 2 столбца

                    User user; //Создаём пользователя. И свойства проверяют входные данные
                    try
                    {
                        user = new User()
                        {
                            UserID = userTelegramId,
                            Name = userName,
                            Surname = userSurname
                        };
                    }
                    catch (Exception ex) //При неправильных данных выдаётся исключение с номером строки с ошибкой
                    {
                        throw new ArgumentException(ex.Message + " Строка " + row.RowIndex);
                    }
                    user.GroupNumber = groupNumber; //Номер группы мы уже проверяли ранее

                    UserFlags userFlags = new UserFlags() { UserID = userTelegramId };
                    UserState userState = new UserState() { UserID = userTelegramId };

                    try
                    {
                        await DataBaseControl.AddEntity(user);
						await DataBaseControl.AddEntity(userFlags);
						await DataBaseControl.AddEntity(userState);
					}
                    catch
                    {
						await DataBaseControl.DeleteEntity<User>(user.UserID);
						await DataBaseControl.DeleteEntity<UserFlags>(userFlags.UserID);
						await DataBaseControl.DeleteEntity<UserState>(userState.UserID);
						throw;
                    }

                    count++; //Увеличили счётчик добавленных пользователей
                }
                return count;
            });
        }

		public static string GetSharedStringItemById(WorkbookPart? workbookPart, string id)
		{
			return workbookPart.SharedStringTablePart.SharedStringTable.
                Elements<SharedStringItem>().
                ElementAt(int.Parse(id)).InnerText;
		}
	}
}
