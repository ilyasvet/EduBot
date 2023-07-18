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
            CaseStage currentStage = StagesControl.Stages[await DataBaseControl.UserCaseTableCommand.GetPoint(userId)];
            //Получили этап, который пользователь только что прошёл, нажав на кнопку

            CaseStage nextStage = await StagesControl.GetNextStage(currentStage, query);
            //Получаем следующий этап, исходя из кнопки, на которую нажал пользователь

            if(nextStage == null)
            {
                if (currentStage is CaseStageEndModule endModule)
                {
                    if(!endModule.IsEndOfCase)
                    {
                        return;
                    }
                    else
                    {
                        if (await DataBaseControl.UserCaseTableCommand.GetHealthPoints(userId) != 0)
                        {
                            return;
                        }
                    }
                }
                await GoOut(userId, botClient);
                return;
            }

            await SetAndMovePoint(userId, nextStage, botClient);
        }
        public static async Task PollAnswerHandlingCase(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
            int attemptNo = await DataBaseControl.UserCaseTableCommand.GetHealthPoints(userId) > 1 ? 1 : 2;
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
            if (message.Text == "/skip")
            {
                SkipCommand s = new();
                await s.Execute(userId, botClient);
                return;
            }

            if (StagesControl.Stages[await DataBaseControl.UserCaseTableCommand.GetPoint(userId)]
                is CaseStageMessage currentStage)
            {
                int attemptNo = await DataBaseControl.UserCaseTableCommand.GetHealthPoints(userId) > 1 ? 1 : 2;
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

                        file = await botClient.GetFileAsync(message.Video.FileId);
                        filePath += "/videos/";
                        fileName += ".mp4";
                        break;
                    default:
                        break;
                }

                if (file.FileSize >= 20 * Math.Pow(2, 20))
                {
                    await botClient.SendTextMessageAsync(userId, Resources.TooBigFile);
                    return;
                    // Если сообщение не соответсвует ответу - ничего не делаем
                }

                using (var stream = new FileStream(filePath + fileName, FileMode.Create))
                {
                    await botClient.DownloadFileAsync(file.FilePath, stream);
                }

                // добавляем птс
                double currentUserRate = await DataBaseControl.UserCaseTableCommand.GetRate(userId);
                currentUserRate += currentStage.Rate;
                await DataBaseControl.UserCaseTableCommand.SetRate(userId, currentUserRate);

                await SetResultsJson(userId, currentStage, currentStage.Rate, attemptNo, null);

                var nextStage = StagesControl.Stages[currentStage.NextStage]; //next уже установлено
                await SetAndMovePoint(userId, nextStage, botClient);
            }
        }

        private static async Task SetResultsJson(
            long userId, CaseStage currentStage, double rate, int attemptNo, int[] optionsIds
            )
        {
            int moduleNumber = currentStage.ModuleNumber;
            int questionNumber = currentStage.Number;

            var time = DateTime.Now - await DataBaseControl.UserCaseTableCommand.GetStartTime(userId);

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
            await DataBaseControl.UserCaseTableCommand.SetOnCourse(userId, false);
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
