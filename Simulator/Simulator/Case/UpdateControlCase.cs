using Simulator.Commands;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System;
using System.IO;
using Simulator.Properties;
using Simulator.Services;
using Simulator.BotControl;

namespace Simulator.Case
{
    internal static class UpdateControlCase
    {
        public static async Task CallbackQueryHandlingCase(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
            int currentPoint = await DataBaseControl.StatsStateTableCommand.
                GetPoint(StagesControl.Stages.CourseName, userId);
            
            if(currentPoint == -1 || query.Data == "ToOut")
            {
                await GoOut(userId, botClient);
                await botClient.DeleteMessageAsync(userId, query.Message.MessageId);
            }
            else if( query.Data == "MoveNext")
            {
                CaseStage currentStage = StagesControl.Stages[currentPoint];
                if (currentStage is CaseStageNone)
                {
                    CaseStage nextStage = StagesControl.Stages[currentStage.NextStage];
                    await SetAndMovePoint(userId, nextStage, botClient);
                }
            }
        }
        public static async Task PollAnswerHandlingCase(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;

            CaseStagePoll currentStage = (CaseStagePoll)StagesControl.
                Stages[await DataBaseControl.StatsStateTableCommand.
                GetPoint(StagesControl.Stages.CourseName, userId)];
            //Так как мы получаем PollAnswer, то очевидно, что текущий этап - опросник

            StagesControl.SetStageForMove(currentStage, answer.OptionIds);
            //По свойствам опросника и ответу определояем свойство NextStage

            double rate = StagesControl.CalculateRatePoll(currentStage, answer.OptionIds);

            if (StagesControl.Stages.DeletePollAfterAnswer)
            {
                int messageId = await DataBaseControl.UserFlagsTableCommand.GetActivePollMessageId(userId);
                await botClient.DeleteMessageAsync(userId, messageId);
            }

            await SaveResultsToStats(userId, currentStage, rate, answer.OptionIds);

            var nextStage = StagesControl.Stages[currentStage.NextStage]; //next уже установлено
            //await botClient.message
            await SetAndMovePoint(userId, nextStage, botClient);
        }

        public static async Task MessageHandlingCase(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;

            if (StagesControl.Stages[
                await DataBaseControl.StatsStateTableCommand.
                GetPoint(StagesControl.Stages.CourseName, userId)
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
                    } catch (Exception ex)
                    {
                        await botClient.SendTextMessageAsync(userId, Resources.WrongFormatFile);
                        return;
                    }
                }

                // добавляем птс

                await SaveResultsToStats(userId, currentStage, currentStage.Rate, null);

                var nextStage = StagesControl.Stages[currentStage.NextStage]; //next уже установлено
                await SetAndMovePoint(userId, nextStage, botClient);
            }
            else
            {
                await botClient.DeleteMessageAsync(userId, message.MessageId);
            }
        }

        private static async Task SaveResultsToStats(
            long userId, CaseStage currentStage, double rate, int[] optionsIds
            )
        {
            string courseName = StagesControl.Stages.CourseName;

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
                await DataBaseControl.StatsStateTableCommand.GetStartTime(courseName, userId)).TotalMinutes;

            stats.Time = time;
            
            int attempt = await DataBaseControl.StatsBaseTableCommand.GetAttemptsUsed(courseName, userId) + 1;

            stats.AttemptNumber = attempt;

            await StatsSetter.SetResults(courseName, userId, stats);
        }

        private static async Task GoOut(long userId, ITelegramBotClient botClient)
        {
            await DataBaseControl.UserFlagsTableCommand.SetOnCourse(userId, false);
            var outCommand = new GoToMainMenuCommand();
            await outCommand.Execute(userId, botClient);
        }
        private async static Task SetAndMovePoint(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            await DataBaseControl.StatsStateTableCommand.
                SetPoint(StagesControl.Stages.CourseName, userId, nextStage.Number);
            //Ставим в базе для пользователя следующий его этап

            await StagesControl.Move(userId, nextStage, botClient);
            //Выдаём пользователю следующий этап
        }
    }
}
