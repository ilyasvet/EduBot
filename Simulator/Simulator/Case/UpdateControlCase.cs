using Simulator.Commands;
using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

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
                await GoOut(userId, botClient);
                return;
            }

            UserCaseTableCommand.SetPoint(userId, nextStage.Number);
            //Ставим в базе для пользователя следующий его этап

            await StagesControl.Move(userId, nextStage, botClient);
            //Выдаём пользователю следующий этап
        }
        public static async Task PollAnswerHandlingCase(PollAnswer answer, ITelegramBotClient botClient)
        {
            long userId = answer.User.Id;
            CaseStagePoll currentStage = StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
            //Так как мы получаем PollAnswer, то очевидно, что текущий этап - опросник

            StagesControl.SetStageForMove(currentStage, answer.OptionIds);
            //По свойствам опросника и ответу определояем свойство NextStage

            double rate = StagesControl.CalculateRatePoll(currentStage, answer.OptionIds);
            double currentUserRate = UserCaseTableCommand.GetRate(userId);
            currentUserRate += rate;
            UserCaseTableCommand.SetRate(userId, currentUserRate);
            //Считаем на основе ответа очки пользователя и добавляем их к общим
            //TODO сделать строковое поле, которое будет показывать количество баллов каждого этапа в модуле.
            //Либо чтобы баллы сразу показывались.

            await StagesControl.AlertNextButton(userId, currentStage, botClient);
            //кнопка для перехода к следующему этапу
        }
        public static async Task MessageHandlingCase(Message message, ITelegramBotClient botClient)
        {
            long userId = message.Chat.Id;
            CaseStageText currentStage = StagesControl.Stages[UserCaseTableCommand.GetPoint(userId)];
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
        private static async Task GoOut(long userId, ITelegramBotClient botClient)
        {
            UserCaseTableCommand.SetOnCourse(userId, false);
            var outCommand = new GoToMainMenuUserCommand();
            await outCommand.Execute(userId, botClient);
        }
    }
}
