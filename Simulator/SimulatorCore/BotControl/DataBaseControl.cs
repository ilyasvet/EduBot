using Simulator.TelegramBotLibrary;
using Simulator.TelegramBotLibrary.StatsTableCommand;
using DbLibrary.CommandHandlers;

namespace Simulator.BotControl
{
    internal static class DataBaseControl
    {
        static DataBaseControl()
        {

            UserStatsControl = new();
            StatsStateTableCommand = new();
            StatsBaseTableCommand = new();
            StatsAnswersTableCommand = new();
            StatsBuilderCommand = new();
        }


        public static async Task<T> GetEntity<T>(object value) where T : new()
        {
            SelectCommandHandler<T> selectCommand = new();
            return await selectCommand.GetEntity(value);
        }
        public static async Task<IEnumerable<T>> GetCollection<T>() where T : new()
        {
            SelectCommandHandler<T> selectCommand = new();
            return await selectCommand.GetAllEntitiesFromTable();
        }
        public static async Task<T> AddEntity<T>(T value) where T : new()
        {
            SelectCommandHandler<T> selectCommand = new();
            return await selectCommand.GetEntity(value);
        }
        public static UserStatsControl UserStatsControl { get; private set; }
        public static StatsAnswersTableCommand StatsAnswersTableCommand { get; private set; }
        public static StatsBaseTableCommand StatsBaseTableCommand { get; private set; }
        public static StatsStateTableCommand StatsStateTableCommand { get; private set; }
        public static StatsBuilderCommand StatsBuilderCommand { get; private set; }

    }
}
