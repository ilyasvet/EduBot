using DbBotLibrary;
using Simulator.Services;
using Simulator.TelegramBotLibrary;
using Simulator.TelegramBotLibrary.StatsTableCommand;

namespace Simulator.BotControl
{
    internal static class DataBaseControl
    {
        static DataBaseControl()
        {
            CommandTable.SetServerName(ControlSystem.serverName);
            CommandTable.SetDataBaseName(ControlSystem.dataBaseName);
            UserTableCommand = new();
            GroupTableCommand = new();
            UserFlagsTableCommand = new();
            CourseTableCommand = new();
            UserStatsControl = new();
            StatsStateTableCommand = new();
            StatsBaseTableCommand = new();
            StatsAnswersTableCommand = new();
            StatsBuilderCommand = new();
        }

        public static UserTableCommand UserTableCommand { get; private set; }
        public static GroupTableCommand GroupTableCommand { get; private set; }
        public static UserFlagsTableCommand UserFlagsTableCommand { get; private set; }
        public static CourseTableCommand CourseTableCommand { get; private set; }
        public static UserStatsControl UserStatsControl { get; private set; }
        public static StatsAnswersTableCommand StatsAnswersTableCommand { get; private set; }
        public static StatsBaseTableCommand StatsBaseTableCommand { get; private set; }
        public static StatsStateTableCommand StatsStateTableCommand { get; private set; }
        public static StatsBuilderCommand StatsBuilderCommand { get; private set; }

    }
}
