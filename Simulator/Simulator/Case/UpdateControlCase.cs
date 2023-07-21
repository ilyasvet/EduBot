using Simulator.Commands;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Simulator.TelegramBotLibrary;
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
            int currentPoint = await DataBaseControl.UserCaseTableCommand.GetPoint(userId);
            
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
            int attemptNo = StagesControl.Stages.AttemptCount
                - await DataBaseControl.UserCaseTableCommand.GetAttempts(userId) + 1;

            CaseStagePoll currentStage = (CaseStagePoll)StagesControl.
                Stages[await DataBaseControl.UserCaseTableCommand.GetPoint(userId)];
            //Так как мы получаем PollAnswer, то очевидно, что текущий этап - опросник

            StagesControl.SetStageForMove(currentStage, answer.OptionIds);
            //По свойствам опросника и ответу определояем свойство NextStage

            double rate = StagesControl.CalculateRatePoll(currentStage, answer.OptionIds);
            double currentUserRate = await DataBaseControl.UserCaseTableCommand.GetRate(userId);
            currentUserRate += rate;
            await DataBaseControl.UserCaseTableCommand.SetRate(userId, currentUserRate);
            //Считаем на основе ответа очки пользователя и добавляем их к общим

            await SetResultsJson(userId, currentStage, rate, attemptNo, answer.OptionIds);

            var nextStage = StagesControl.Stages[currentStage.NextStage]; //next уже установлено
            //await botClient.message
            await SetAndMovePoint(userId, nextStage, botClient);
        }

        public static async Task MessageHandlingCase(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;

            if (StagesControl.Stages[await DataBaseControl.UserCaseTableCommand.GetPoint(userId)]
                is CaseStageMessage currentStage)
            {
                int attemptNo = StagesControl.Stages.AttemptCount
                - await DataBaseControl.UserCaseTableCommand.GetAttempts(userId) + 1;

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
                double currentUserRate = await DataBaseControl.UserCaseTableCommand.GetRate(userId);
                currentUserRate += currentStage.Rate;
                await DataBaseControl.UserCaseTableCommand.SetRate(userId, currentUserRate);

                await SetResultsJson(userId, currentStage, currentStage.Rate, attemptNo, null);

                var nextStage = StagesControl.Stages[currentStage.NextStage]; //next уже установлено
                await SetAndMovePoint(userId, nextStage, botClient);
            }
            else
            {
                await botClient.DeleteMessageAsync(userId, message.MessageId);
            }
        }

        private static async Task SetResultsJson(
            long userId, CaseStage currentStage, double rate, int attemptNo, int[] optionsIds
            )
        {
            int moduleNumber = currentStage.ModuleNumber;
            int questionNumber = currentStage.Number;

            var time = DateTime.Now - await DataBaseControl.UserFlagsTableCommand.GetStartTime(userId);

            StageResults results = new StageResults();
            results.Time = time;
            results.Rate = rate;
           
            //варианты ответа
            if (currentStage is CaseStagePoll)
            {
                string jsonUserAnswers = "";
                foreach (int option in optionsIds)
                {
                    jsonUserAnswers += $"{option + 1};";
                    // счёт идёт от 0, а надо от 1, поэтому +1
                }
                results.Answers = jsonUserAnswers; 
            }
            await CaseJsonCommand.AddValueToJsonFile(userId, (moduleNumber, questionNumber), results, attemptNo);
        }

        private static async Task GoOut(long userId, ITelegramBotClient botClient)
        {
            await DataBaseControl.UserFlagsTableCommand.SetOnCourse(userId, false);
            var outCommand = new GoToMainMenuCommand();
            await outCommand.Execute(userId, botClient);
        }
        private async static Task SetAndMovePoint(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            await DataBaseControl.UserCaseTableCommand.SetPoint(userId, nextStage.Number);
            //Ставим в базе для пользователя следующий его этап

            await StagesControl.Move(userId, nextStage, botClient);
            //Выдаём пользователю следующий этап
        }
    }
}
