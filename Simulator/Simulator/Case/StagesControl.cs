using Simulator.Models;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Simulator.Case
{
    internal static class StagesControl
    {
        public static StageList Stages { get; set; }
        
        public static double CalculateRatePoll(CaseStagePoll stage, int[] answers)
        {
            double rate = 0;
            foreach (var answer in answers)
            {
                rate += stage.PossibleRate[answer];
            }
            return rate;
        }
        public static void SetStageForMove(CaseStagePoll stage, int[] answers)
        {
            if(stage.ConditionalMove)
            {
                stage.NextStage = stage.MovingNumbers[answers[0]];
            } 
        }
        public static async Task MoveNext(long userId, CaseStage nextStage, ITelegramBotClient botClient)
        {
            switch (nextStage)
            {
                case CaseStagePoll poll:
                    await botClient.SendPollAsync(
                        chatId: userId,
                        question: poll.TextBefore,
                        options: poll.Options,
                        isAnonymous: false,
                        allowsMultipleAnswers: poll.ManyAnswers);
                    break;
                default:
                    break;
            }
        }
    }
}
