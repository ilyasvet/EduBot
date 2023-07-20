using DbBotLibrary;
using System;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class UserCaseTableCommand : CommandTable
    {
        public async Task SetPoint(long userId, int number)
        {
            string commandText = $"UPDATE UserCourseState SET Point = {number} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetPoint(long userId)
        {
            string commandText = $"SELECT Point FROM UserCourseState WHERE UserID = {userId}";
            
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

        public async Task SetRate(long userId, double currentUserRate)
        {
            string commandText = $"UPDATE UserCourseState SET Rate = {currentUserRate} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<double> GetRate(long userId)
        {
            string commandText = $"SELECT Rate FROM UserCourseState WHERE UserID = {userId}";
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

        public async Task SetHealthPoints(long userId, int healthPoints = 3)
        {
            string commandText = $"UPDATE UserCourseState SET HealthPoints = {healthPoints} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetHealthPoints(long userId)
        {
            string commandText = $"SELECT HealthPoints FROM UserCourseState WHERE UserID = {userId}";

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

        public async Task SetStartCaseTime(long userId, DateTime time)
        {
            string commandText = $"UPDATE UserCourseState SET StartCourse = '{time}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<DateTime> GetStartCaseTime(long userId)
        {
            string commandText = $"SELECT StartCourse FROM UserCourseState WHERE UserID = {userId}";

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

        public async Task SetEndCaseTime(long userId, DateTime time)
        {
            string commandText = $"UPDATE UserCourseState SET EndCourse = '{time}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }
        public async Task<DateTime> GetEndCaseTime(long userId)
        {
            string commandText = $"SELECT EndCourse FROM UserCourseState WHERE UserID = {userId}";

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
