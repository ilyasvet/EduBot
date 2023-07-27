using DbBotLibrary;
using Simulator.Services;
using Simulator.TelegramBotLibrary;

namespace Simulator.BotControl
{
    internal static class DataBaseControl
    {
        static DataBaseControl()
        {
            CommandTable.SetServerName(ControlSystem.serverName);
            CommandTable.SetDataBaseName(ControlSystem.dataBaseName);
            UserTableCommand = new();
            UserCaseTableCommand = new();
            GroupTableCommand = new();
            UserFlagsTableCommand = new();
            CourseTableCommand = new();
            UserStatsControl = new();
        }

        public static UserTableCommand UserTableCommand { get; private set; }
        public static StatsStateTableCommand UserCaseTableCommand { get; private set; }
        public static GroupTableCommand GroupTableCommand { get; private set; }
        public static UserFlagsTableCommand UserFlagsTableCommand { get; private set; }
        public static CourseTableCommand CourseTableCommand { get; private set; }
        public static UserStatsControl UserStatsControl { get; private set; }
    }
}
