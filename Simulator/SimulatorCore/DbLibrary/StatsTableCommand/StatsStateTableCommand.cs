namespace SimulatorCore.DbLibrary.StatsTableCommand
{
    public class StatsStateTableCommand : CommandTable
    {
        private const string TABLE_TYPE = "State";
        public async Task SetPoint(string courseName, long userId, int number)
        {
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} SET Point = {number} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetPoint(string courseName, long userId)
        {
            string commandText = $"SELECT Point FROM Stats{courseName}{TABLE_TYPE} WHERE UserID = {userId}";

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

        public async Task SetRate(string courseName, long userId, double currentUserRate)
        {
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} SET Rate = {currentUserRate} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<double> GetRate(string courseName, long userId)
        {
            string commandText = $"SELECT Rate FROM Stats{courseName}{TABLE_TYPE} WHERE UserID = {userId}";
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

        public async Task SetAttempts(string courseName, long userId, int attempts)
        {
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} SET Attempts = {attempts} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetAttempts(string courseName, long userId)
        {
            string commandText = $"SELECT Attempts FROM Stats{courseName}{TABLE_TYPE} WHERE UserID = {userId}";

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

        public async Task SetExtraAttempt(string courseName, long userId, bool extraAttempt)
        {
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} SET ExtraAttempt = '{extraAttempt}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<bool> GetExtraAttempt(string courseName, long userId)
        {
            string commandText = $"SELECT ExtraAttempt FROM Stats{courseName}{TABLE_TYPE} WHERE UserID = {userId} AND ExtraAttempt IS NOT NULL";

            bool result = (bool)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (bool)reader[0];
                }
                return null;
            });

            return result;
        }

        public async Task SetStartTime(string courseName, long userId, DateTime time)
        {
            string commandText = $"UPDATE Stats{courseName}{TABLE_TYPE} SET StartTime = '{time}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<DateTime> GetStartTime(string courseName, long userId)
        {
            string commandText = $"SELECT StartTime FROM Stats{courseName}{TABLE_TYPE} WHERE UserID = {userId}";

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
