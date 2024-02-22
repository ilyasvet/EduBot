using EduBot.BotControl;
using EduBotCore.DbLibrary.StatsTableCommand;

namespace EduBot.Case
{
    internal class StatsSetter
    {
        public static async Task SetResults(string CourseName, long userId, StageResults stats)
        {
            await DataBaseControl.StatsAnswersTableCommand.SetStatistics(CourseName, userId, stats);
            double currentRate = await DataBaseControl.StatsStateTableCommand.GetRate(CourseName, userId);
            currentRate += stats.Rate;
            await DataBaseControl.StatsBaseTableCommand.SetAttemptRate(
                CourseName, userId, stats.AttemptNumber, currentRate
                );
            await DataBaseControl.StatsStateTableCommand.SetRate(CourseName, userId, currentRate);
        }
    }
}
