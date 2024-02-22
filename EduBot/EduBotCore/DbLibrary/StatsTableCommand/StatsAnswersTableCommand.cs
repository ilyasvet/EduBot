namespace EduBotCore.DbLibrary.StatsTableCommand
{
    public class StatsAnswersTableCommand : CommandTable
    {
        private const string TABLE_NAME_ANSWERS = "answers";
        private const string TABLE_NAME_RATE = "rate";
        private const string TABLE_NAME_TIME = "time";

        public async Task SetStatistics(string courseName, long userId, StageResults stageResults)
        {
            string commandText = $"UPDATE stats{courseName}{TABLE_NAME_RATE} SET " +
                $"P{stageResults.StageNumber}M{stageResults.ModuleNumber}A{stageResults.AttemptNumber} = " +
                $"{stageResults.Rate} " +
                $"WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);

            commandText = $"UPDATE stats{courseName}{TABLE_NAME_TIME} SET " +
                $"P{stageResults.StageNumber}M{stageResults.ModuleNumber}A{stageResults.AttemptNumber} = " +
                $"{stageResults.Time} " +
                $"WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);

            if (stageResults.OptionsIds != null)
            {
                string formatAnswers = string.Join(";", stageResults.OptionsIds);
                commandText = $"UPDATE stats{courseName}{TABLE_NAME_ANSWERS} SET " +
                    $"P{stageResults.StageNumber}M{stageResults.ModuleNumber}A{stageResults.AttemptNumber} = " +
                    $"'{formatAnswers}' " +
                    $"WHERE UserID = {userId}";
                await ExecuteNonQueryCommand(commandText);
            }
        }

        public async Task<StageResults> GetStatistics(string courseName, long userId)
        {
            string commandText = $"SELECT AttemptsUsed FROM stats{courseName}{TABLE_NAME_RATE} " +
                $"WHERE UserID = {userId}";

            int result = (int)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (int)reader[0];
                }
                return null;
            });
            // TODO доделать
            return default;
        }
    }
}
