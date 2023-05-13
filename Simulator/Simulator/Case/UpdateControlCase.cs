using Simulator.Commands;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Simulator.TelegramBotLibrary;
using System;

namespace Simulator.Case
{
    internal static class UpdateControlCase
    {
        public static async Task CallbackQueryHandlingCase(CallbackQuery query, ITelegramBotClient botClient)
        {
            long userId = query.Message.Chat.Id;
            CaseStage currentStage = StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
            //Получили этап, который пользователь только что прошёл, нажав на кнопку

            CaseStage nextStage = StagesControl.GetNextStage(currentStage, query);
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
                        if (UserCaseTableCommand.GetHealthPoints(userId) != 0)
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
            int attemptNo = UserCaseTableCommand.GetHealthPoints(userId) > 1 ? 1 : 2;
            CaseStagePoll currentStage = (CaseStagePoll)StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
            //Так как мы получаем PollAnswer, то очевидно, что текущий этап - опросник

            StagesControl.SetStageForMove(currentStage, answer.OptionIds);
            //По свойствам опросника и ответу определояем свойство NextStage

            double rate = StagesControl.CalculateRatePoll(currentStage, answer.OptionIds);
            double currentUserRate = UserCaseTableCommand.GetRate(userId);
            currentUserRate += rate;
            UserCaseTableCommand.SetRate(userId, currentUserRate);
            //Считаем на основе ответа очки пользователя и добавляем их к общим

            await SetResultsJson(userId, currentStage, rate, attemptNo, answer.OptionIds);

            var nextStage = StagesControl.Stages[currentStage.NextStage]; //next уже установлено
            //await botClient.message
            await SetAndMovePoint(userId, nextStage, botClient);
        }

        public static async Task MessageHandlingCase(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;
            CaseStageText currentStage = (CaseStageText)StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
            switch (currentStage.MessageTypeAnswer)
            {
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    //StagesControl.VideoHandling(message.Video);
                    //TODO узнать, как обрабатывать видео и ставить баллы
                    break;
                default:
                    break;
            }
        }

        private static async Task SetResultsJson(long userId, CaseStagePoll currentStage, double rate, int attemptNo, int[] optionsIds)
        {
            int moduleNumber = currentStage.ModuleNumber;
            int questionNumber = currentStage.Number;
            var time = DateTime.Now - UserCaseTableCommand.GetStartTime(userId);

            //время и баллы записываем
            await UserCaseJsonCommand.AddValueToJsonFile(userId, (moduleNumber, questionNumber), rate, attemptNo);
            await UserCaseJsonCommand.AddValueToJsonFile(userId, (moduleNumber, questionNumber), time, attemptNo);

            //варианты ответа
            string jsonUserAnswers = "";
            foreach (int option in optionsIds)
            {
                jsonUserAnswers += $"{option};";
            }
            await UserCaseJsonCommand.AddValueToJsonFile(userId, (moduleNumber, questionNumber), jsonUserAnswers, attemptNo);
        }

        private static async Task GoOut(long userId, ITelegramBotClient botClient)
        {
            UserCaseTableCommand.SetOnCourse(userId, false);
            var outCommand = new GoToMainMenuUserCommand();
            await outCommand.Execute(userId, botClient);
        }
        private async static Task SetAndMovePoint(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            UserCaseTableCommand.SetPoint(userId, nextStage.Number);
            //Ставим в базе для пользователя следующий его этап

            await StagesControl.Move(userId, nextStage, botClient);
            //Выдаём пользователю следующий этап
        }
    }
}
