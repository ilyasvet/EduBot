using DbBotLibrary;
using System.Threading.Tasks;
using System;

namespace Simulator.TelegramBotLibrary.StatsTableCommand
{
    public class StatsBaseTableCommand : CommandTable
    {
        private const string TABLE_TYPE = "Base";

        public async Task SetAttemptsUsed(string courseName, long userId)
        {
            int oldValue = await GetAttemptsUsed(courseName, userId);
            oldValue++;

            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} " +
                $"SET AttemptsUsed = {oldValue} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetAttemptsUsed(string courseName, long userId)
        {
            string commandText = $"SELECT AttemptsUsed FROM Stats{courseName}{TABLE_TYPE} " +
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
            double oldValue = await GetAttemptRate(courseName, userId, attemptNumber);
            oldValue += rate;

            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} " +
                $"SET RateAttempt{attemptNumber} = {oldValue} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<double> GetAttemptRate(string courseName, long userId, int attemptNumber)
        {
            string commandText = $"SELECT RateAttempt{attemptNumber} FROM Stats{courseName}{TABLE_TYPE} " +
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
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} " +
                $"SET StartCourse = '{time}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<DateTime> GetStartCaseTime(string courseName, long userId)
        {
            string commandText = $"SELECT StartCourse FROM Stats{courseName}{TABLE_TYPE} " +
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
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} " +
                $"SET EndCourse = '{time}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }
        public async Task<DateTime> GetEndCaseTime(string courseName, long userId)
        {
            string commandText = $"SELECT EndCourse FROM Stats{courseName}{TABLE_TYPE} " +
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
