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


        public static async Task<T> GetEntity<T>(object primaryKey) where T : new()
        {
            SelectCommandHandler<T> selectCommand = new();
            return await selectCommand.GetEntity(primaryKey);
        }
        public static async Task<IEnumerable<T>> GetCollection<T>() where T : new()
        {
            SelectCommandHandler<T> selectCommand = new();
            return await selectCommand.GetAllEntitiesFromTable();
        }
        public static async Task<int> AddEntity<T>(T value) where T : new()
        {
            InsertCommandHandler<T> insertCommand = new();
            return await insertCommand.AddEntity(value);
        }
        public static async Task<int> UpdateEntity<T>(object primaryKey, T entity) where T : new()
        {
            UpdateCommandHandler<T> updateCommand = new();
            return await updateCommand.UpdateEntity(primaryKey, entity);
        }
		public static async Task<int> DeleteEntity<T>(object primaryKey) where T : new()
		{
			DeleteCommandHandler<T> deleteCommand = new();
			return await deleteCommand.DeleteEntity(primaryKey);
		}


		public static UserStatsControl UserStatsControl { get; private set; }
        public static StatsAnswersTableCommand StatsAnswersTableCommand { get; private set; }
        public static StatsBaseTableCommand StatsBaseTableCommand { get; private set; }
        public static StatsStateTableCommand StatsStateTableCommand { get; private set; }
        public static StatsBuilderCommand StatsBuilderCommand { get; private set; }

    }
}
