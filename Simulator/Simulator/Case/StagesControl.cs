using Simulator.Models;

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
    }
}
