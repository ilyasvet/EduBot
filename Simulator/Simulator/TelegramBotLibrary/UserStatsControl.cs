using DbBotLibrary;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public enum StatsTableType
    {
        Rate,
        Answers,
        Time,
        Base,
        State,
    }
    public class UserStatsControl : CommandTable
    {
        private List<string> _tableNames;
        private void FillTableNames(string courseName)
        {
            foreach (string typeName in Enum.GetNames(typeof(StatsTableType)))
            {
                string pattern = $"Stats{courseName}{typeName}";
                _tableNames.Add(pattern);
            }
        }
        public async Task DeleteStatsTables(string courseName)
        {
            FillTableNames(courseName);
            string commandText = string.Empty;
            foreach(string tableName in _tableNames)
            {
                commandText += $"DROP TABLE {tableName}\n";
            }
            await ExecuteNonQueryCommand(commandText);
        }

        internal async Task MakeStatsTables(StageList stages)
        {
            FillTableNames(stages.CourseName);

        }
    }
}
