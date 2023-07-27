using DbBotLibrary;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
        private const int COUNT_TABLES_WITH_ALL_STAGES = 3;
        private List<string> FillTableNames(string courseName)
        {
            List<string> tableNames = new List<string>();
            foreach (string typeName in Enum.GetNames(typeof(StatsTableType)))
            {
                string pattern = $"Stats{courseName}{typeName}";
                tableNames.Add(pattern);
            }
            return tableNames;
        }
        public async Task DeleteStatsTables(string courseName)
        {
            List<string> tableNames = FillTableNames(courseName);
            string commandText = string.Empty;
            foreach(string tableName in tableNames)
            {
                commandText += $"DROP TABLE {tableName}\n";
            }
            await ExecuteNonQueryCommand(commandText);
        }

        internal async Task MakeStatsTables(StageList stages)
        {
            List<string> tableNames = FillTableNames(stages.CourseName);
            for (int i = 0; i < COUNT_TABLES_WITH_ALL_STAGES; i++)
            {
                await MakeStatsAllStages(stages, tableNames[i]);
            }
            await MakeBaseStatsTable(stages.AttemptCount, tableNames[3]);
            await MakeStateStatsTable(tableNames[4]);
            await FillTablesDefaultInformation(tableNames);
            await FillBaseStatsTable(stages.AttemptCount, stages.ExtraAttempt, tableNames[3]);
        }

        private async Task FillBaseStatsTable(int attemptCount, bool extraAttempt, string tableName)
        {
            string commandText = $"UPDATE {tableName} SET ExtraAttempt = '{extraAttempt}', Attempts = {attemptCount}";
            await ExecuteNonQueryCommand(commandText);
        }

        private async Task FillTablesDefaultInformation(List<string> tableNames)
        {
            string commandText;
            foreach(string tableName in tableNames)
            {
                commandText = $"INSERT INTO {tableName} (UserID) SELECT UserID from Users";
                await ExecuteNonQueryCommand(commandText);
            }
        }

        private async Task MakeStateStatsTable(string tableName)
        {
            StringBuilder commandText = new StringBuilder($"CREATE TABLE {tableName} (\n");

            commandText.AppendLine("UserID INT NOT NULL PRIMARY KEY REFERENCES Users(UserID),");
            commandText.AppendLine("Point INT NOT NULL DEFAULT 0,");
            commandText.AppendLine("ExtraAttempt BIT NULL,");
            commandText.AppendLine("Attempts INT NULL,");
            commandText.AppendLine("Rate FLOAT NOT NULL DEFAULT 0,");
            commandText.AppendLine("StartTime DATETIME NULL,");

            commandText.Append(')');

            await ExecuteNonQueryCommand(commandText.ToString());
        }

        private async Task MakeBaseStatsTable(int attemptCount, string tableName)
        {
            StringBuilder commandText = new StringBuilder($"CREATE TABLE {tableName} (\n");

            commandText.AppendLine("UserID INT NOT NULL PRIMARY KEY REFERENCES Users(UserID),");
            commandText.AppendLine($"StartCourseTime DATETIME NULL,");
            commandText.AppendLine($"EndCourseTime DATETIME NULL,");
            commandText.AppendLine($"AttemptsUsed INT NOT NULL DEFAULT 0,");

            for (int i = 1; i <= attemptCount; i++)
            {
                commandText.AppendLine($"RateAttempt{i} FLOAT NOT NULL DEFAULT 0,");
            }

            commandText.Append(')');

            await ExecuteNonQueryCommand(commandText.ToString());
        }

        // Создаёт таблицу, столбцы которой - сначала все вопросы первой попытки,
        // Потом второй, и так далее, в зависимости от попыток
        private async Task MakeStatsAllStages(StageList stages, string tableName)
        {
            StringBuilder commandText = new StringBuilder($"CREATE TABLE {tableName} (\n");

            commandText.AppendLine("UserID INT NOT NULL PRIMARY KEY REFERENCES Users(UserID),");
            
            List<StringBuilder> commandTextAnyAttempt = new();
            for (int i = 0; i < stages.AttemptCount; i++)
            {
                commandTextAnyAttempt[i] = new StringBuilder();
            }

            for (int j = 0; j < stages.GetLastStageIndex(); j++)
            {
                CaseStage currentStage = stages[j];
                for (int i = 0; i < stages.AttemptCount; i++)
                {
                    commandTextAnyAttempt[i].AppendLine($"P{currentStage.Number}M{currentStage.ModuleNumber}A{i+1}" +
                        $" FLOAT NOT NULL DEFAULT 0,");
                }
            }

            for (int i = 0; i < stages.AttemptCount; i++)
            {
                commandText.Append(commandTextAnyAttempt[i]);
            }

            commandText.Append(")");

            await ExecuteNonQueryCommand(commandText.ToString());
        }
    }
}
