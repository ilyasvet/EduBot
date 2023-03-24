using Simulator.Properties;
using Simulator.TelegramBotLibrary;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = Simulator.Models.User;
using Excel = Microsoft.Office.Interop.Excel;

namespace Simulator.BotControl
{
    public static class CommandExecuteExtensionFile
    {
        public static Task Execute(long userId, ITelegramBotClient botClient, Document message)
        {
            return Task.Run(() =>
            {
                DialogState state = UserTableCommand.GetDialogState(userId);
                switch (state)
                {
                    case DialogState.None:
                        break;
                    case DialogState.SendingGroupFile:
                        AddNewUsersTable(userId, botClient, message);
                        break;
                    default:
                        break;
                }
            });
        }

        private static void AddNewUsersTable(long userId, ITelegramBotClient botClient, Document message)
        {
            User user = UserTableCommand.GetUserById(userId);
            if (!user.IsAdmin)
            {
                botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: Resources.NotAdmin);
            }
            string FileName = message.FileName;
            object rOnly = true;
            object SaveChanges = false;
            object MissingObj = System.Reflection.Missing.Value;

            Excel.Application app = new Excel.Application();
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Sheets sheets = null;
            try
            {
                workbooks = app.Workbooks;
                workbook  = workbooks.Open(FileName, MissingObj, rOnly, MissingObj, MissingObj,
                                            MissingObj, MissingObj, MissingObj, MissingObj, MissingObj,
                                            MissingObj, MissingObj, MissingObj, MissingObj, MissingObj);

                // Получение всех страниц докуента
                sheets = workbook.Sheets;
                            
                foreach(Excel.Worksheet worksheet in sheets)
                {
                    // Получаем диапазон используемых на странице ячеек
                    Excel.Range UsedRange = worksheet.UsedRange;
                    // Получаем строки в используемом диапазоне
                    Excel.Range urRows = UsedRange.Rows;
                    // Получаем столбцы в используемом диапазоне
                    Excel.Range urColums = UsedRange.Columns;

                    // Количества строк и столбцов
                    int RowsCount = urRows.Count;
                    int ColumnsCount = urColums.Count;
                    for(int i = 1; i <= RowsCount; i++)
                    {
                        for(int j = 1; j <= ColumnsCount; j++)
                        {
                            Excel.Range CellRange = UsedRange.Cells[i, j]; //TODO переделать обработку
                            // Получение текста ячейки
                            string CellText = (CellRange == null || CellRange.Value2 == null) ? null :
                                                (CellRange as Excel.Range).Value2.ToString();

                            if(CellText != null)
                            {
                                /* Обработка текста */
                            }
                        }
                    }
                    // Очистка неуправляемых ресурсов на каждой итерации
                    if (urRows != null) Marshal.ReleaseComObject(urRows);
                    if (urColums != null) Marshal.ReleaseComObject(urColums);
                    if (UsedRange != null) Marshal.ReleaseComObject(UsedRange);
                    if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                }
            } catch (Exception ex)
            {
                /* Обработка исключений */
            }
            finally
            {
                /* Очистка оставшихся неуправляемых ресурсов */
                if (sheets != null) Marshal.ReleaseComObject(sheets);
                if (workbook != null)
                {
                    workbook.Close(SaveChanges);
                    Marshal.ReleaseComObject(workbook);
                    workbook = null;
                }

                if (workbooks != null)
                {
                    workbooks.Close();
                    Marshal.ReleaseComObject(workbooks);
                    workbooks = null;
                }
                if (app != null)
                {
                    app.Quit();
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
            }
        }
    }
}