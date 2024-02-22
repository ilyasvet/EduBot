using System.Globalization;

namespace EduBotCore.DbLibrary.StatsTableCommand
{
    public class StatsBaseTableCommand : CommandTable
    {
        private const string TABLE_TYPE = "base";

        public async Task SetAttemptsUsed(string courseName, long userId)
        {
            int oldValue = await GetAttemptsUsed(courseName, userId);
            oldValue++;

            string commandText = $"UPDATE stats{courseName}{TABLE_TYPE} " +
                $"SET AttemptsUsed = {oldValue} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetAttemptsUsed(string courseName, long userId)
        {
            string commandText = $"SELECT AttemptsUsed FROM stats{courseName}{TABLE_TYPE} " +
                $"WHERE UserID = {userId}";

            int result = (int)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (int)reader[0];
                }
                return null;
            });

            return result;
        }

        public async Task SetAttemptRate(string courseName, long userId, int attemptNumber, double rate)
        {
            string commandText = $"UPDATE stats{courseName}{TABLE_TYPE} " +
                $"SET RateAttempt{attemptNumber} = {rate} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<double> GetAttemptRate(string courseName, long userId, int attemptNumber)
        {
            string commandText = $"SELECT RateAttempt{attemptNumber} FROM stats{courseName}{TABLE_TYPE} " +
                $"WHERE UserID = {userId}";

            double result = (double)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (double)reader[0];
                }
                return null;
            });

            return result;
        }

        public async Task SetStartCaseTime(string courseName, long userId, DateTime time)
        {
            if (await StartTimeIsNull(courseName, userId))
            {
                var timeStr = time.ToString("yyyy-MM-dd HH:mm:ss");
			    string commandText = $"UPDATE stats{courseName}{TABLE_TYPE} " +
                    $"SET StartCourseTime = '{timeStr}' WHERE UserID = {userId}";
                await ExecuteNonQueryCommand(commandText);
            }
        }

        private async Task<bool> StartTimeIsNull(string courseName, long userId)
        {
            string commandText = $"SELECT COUNT(UserID) FROM stats{courseName}{TABLE_TYPE} " +
                $"WHERE StartCourseTime IS NULL AND UserID = {userId}";

            bool result = (bool)await ExecuteReaderCommand(commandText, (reader) =>
            {
                reader.Read();
                return (long)reader[0] != 0;
            });
            return result;
        }

        public async Task<DateTime> GetStartCaseTime(string courseName, long userId)
        {
            string commandText = $"SELECT StartCourseTime FROM stats{courseName}{TABLE_TYPE} " +
                $"WHERE UserID = {userId}";

            DateTime result = (DateTime)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (DateTime)reader[0];
                }
                return null;
            });

            return result;
        }

        public async Task SetEndCaseTime(string courseName, long userId, DateTime time)
        {
            string commandText = $"UPDATE stats{courseName}{TABLE_TYPE} " +
                $"SET EndCourseTime = '{time.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }
        public async Task<DateTime> GetEndCaseTime(string courseName, long userId)
        {
            string commandText = $"SELECT EndCourseTime FROM stats{courseName}{TABLE_TYPE} " +
                $"WHERE UserID = {userId}";

            DateTime result = (DateTime)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (DateTime)reader[0];
                }
                return null;
            });

            return result;
        }
    }
}
