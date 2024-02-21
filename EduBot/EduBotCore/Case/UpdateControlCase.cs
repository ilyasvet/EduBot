using Simulator.Commands;
using Simulator.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using EduBotCore.Properties;
using Simulator.Services;
using Simulator.BotControl;
using EduBotCore.DbLibrary.StatsTableCommand;
using EduBotCore.Models.DbModels;
using EduBotCore.Case;

namespace Simulator.Case
{
    internal static class UpdateControlCase
    {
        public static async Task CallbackQueryHandlingCase(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
			UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
			int currentPoint = await DataBaseControl.StatsStateTableCommand.
                GetPoint(userFlags.CurrentCourse, userId);
            
            if(currentPoint == -1 || query.Data == "ToOut")
            {
                await GoOut(userId, botClient);
                await botClient.DeleteMessageAsync(userId, query.Message.MessageId);
            }
            else if( query.Data == "MoveNext")
            {
                CaseStage currentStage = CoursesControl.Courses[userFlags.CurrentCourse][currentPoint];
                if (currentStage is CaseStageNone)
                {
                    CaseStage nextStage = CoursesControl.Courses[userFlags.CurrentCourse][currentStage.NextStage];
                    await SetAndMovePoint(userFlags, nextStage, botClient);
                }
            }
        }
        public static async Task PollAnswerHandlingCase(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
			UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);

			CaseStagePoll currentStage = (CaseStagePoll)CoursesControl.Courses[userFlags.CurrentCourse]
                [await DataBaseControl.StatsStateTableCommand.
                GetPoint(userFlags.CurrentCourse, userId)];
            //Так как мы получаем PollAnswer, то очевидно, что текущий этап - опросник

            StagesControl.SetStageForMove(currentStage, answer.OptionIds);
            //По свойствам опросника и ответу определояем свойство NextStage

            double rate = StagesControl.CalculateRatePoll(currentStage, answer.OptionIds);

            if (CoursesControl.Courses[userFlags.CurrentCourse].DeletePollAfterAnswer)
            {
                await botClient.DeleteMessageAsync(userId, userFlags.ActivePollMessageId);
            }

            await SaveResultsToStats(userFlags, currentStage, rate, answer.OptionIds);

            var nextStage = CoursesControl.Courses[userFlags.CurrentCourse][currentStage.NextStage]; //next уже установлено
            //await botClient.message
            await SetAndMovePoint(userFlags, nextStage, botClient);
        }

        public static async Task MessageHandlingCase(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;
			UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);

			if (CoursesControl.Courses[userFlags.CurrentCourse][
                await DataBaseControl.StatsStateTableCommand.
                GetPoint(userFlags.CurrentCourse, userId)
                ]
                is CaseStageMessage currentStage)
            {
                string fileName = $"{userId}-{currentStage.Number}";
                string filePath = ControlSystem.messageAnswersDirectory;
                Telegram.Bot.Types.File file = null;

                switch (currentStage.MessageTypeAnswer)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Video:

                        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Video)
                        {
                            await botClient.SendTextMessageAsync(userId, Resources.SendVideo);
                            return;
                            // Если сообщение не соответсвует ответу - ничего не делаем
                        }

                        if (message.Video.FileSize >= 20 * Math.Pow(2, 20))
                        {
                            await botClient.SendTextMessageAsync(userId, Resources.TooBigFile);
                            return;
                            // Если сообщение не соответсвует ответу - ничего не делаем
                        }

                        file = await botClient.GetFileAsync(message.Video.FileId);
                        filePath += "/videos/";
                        fileName += ".mp4";
                        break;
                    default:
                        break;
                }

                using (var stream = new FileStream(filePath + fileName, FileMode.Create))
                {
                    try
                    {
                        await botClient.DownloadFileAsync(file.FilePath, stream);
                    }
                    catch (Exception ex)
                    {
                        await botClient.SendTextMessageAsync(userId, Resources.WrongFormatFile);
                        return;
                    }
                }

                // добавляем птс

                await SaveResultsToStats(userFlags, currentStage, currentStage.Rate, null);

                var nextStage = CoursesControl.Courses[userFlags.CurrentCourse][currentStage.NextStage]; //next уже установлено
                await SetAndMovePoint(userFlags, nextStage, botClient);
            }
            else
            {
                await botClient.DeleteMessageAsync(userId, message.MessageId);
            }
        }

        private static async Task SaveResultsToStats(
            UserFlags userFlags, CaseStage currentStage, double rate, int[] optionsIds
            )
        {
            string courseName = userFlags.CurrentCourse;

            // Мы должны сохранить (должны знать номер попытки, модуля и вопроса)
            // Ответы
            // Время
            // Очки

            // Обновить общие очки
            // Обновить общие очки за попытку

            StageResults stats;
            stats.Rate = rate;
            stats.ModuleNumber = currentStage.ModuleNumber;
            stats.StageNumber = currentStage.Number;
            stats.OptionsIds = optionsIds;

            double time = (DateTime.Now - 
                await DataBaseControl.StatsStateTableCommand.GetStartTime(courseName, userFlags.UserID)).TotalMinutes;

            stats.Time = time;
            
            int attempt = await DataBaseControl.StatsBaseTableCommand.GetAttemptsUsed(courseName, userFlags.UserID) + 1;

            stats.AttemptNumber = attempt;

            await StatsSetter.SetResults(courseName, userFlags.UserID, stats);
        }

        private static async Task GoOut(long userId, ITelegramBotClient botClient)
        {
            UserFlags userFlags = await DataBaseControl.GetEntity<UserFlags>(userId);
            userFlags.CurrentCourse = null;
            await DataBaseControl.UpdateEntity(userId, userFlags);

            var outCommand = new GoToMainMenuCommand();
            await outCommand.Execute(userId, botClient);
        }
        private async static Task SetAndMovePoint(UserFlags userFlags, CaseStage nextStage, ITelegramBotClient botClient)
        {
            await DataBaseControl.StatsStateTableCommand.
                SetPoint(userFlags.CurrentCourse, userFlags.UserID, nextStage.Number);
            //Ставим в базе для пользователя следующий его этап

            await StagesControl.Move(userFlags.UserID, nextStage, botClient);
            //Выдаём пользователю следующий этап
        }
    }
}
