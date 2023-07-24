using DbBotLibrary;
using System;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class UserFlagsTableCommand : CommandTable
    {
        public async Task SetMessageStartDialogId(long userId, int messageId)
        {
            string commandText = $"UPDATE UserFlags SET StartDialogId = {messageId} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetMessageStartDialogId(long userId)
        {
            string commandText = $"SELECT StartDialogId FROM UserFlags WHERE UserID = {userId}";

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

        public async Task SetOnCourse(long userId, bool boolOnCourse)
        {
            string commandText = $"UPDATE UserFlags SET OnCourse = '{boolOnCourse}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<bool> IsOnCourse(long userId)
        {
            string commandText = $"SELECT OnCourse FROM UserFlags WHERE UserID = {userId}";

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

        public async Task SetCalculatedEndStage(long userId, bool boolOnCourse)
        {
            string commandText = $"UPDATE UserFlags SET CalculatedEndStage = '{boolOnCourse}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<bool> IsCalculatedEndStage(long userId)
        {
            string commandText = $"SELECT CalculatedEndStage FROM UserFlags WHERE UserID = {userId}";

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
        
        public async Task SetStartTime(long userId, DateTime time)
        {
            string commandText = $"UPDATE UserFlags SET StartQuestTime = '{time}' WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<DateTime> GetStartTime(long userId)
        {
            string commandText = $"SELECT StartQuestTime FROM UserFlags WHERE UserID = {userId}";

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

        public async Task SetActivePollMessageId(long userId, int activePollMessageId)
        {
            string commandText = $"UPDATE UserFlags SET ActivePollMessageId = {activePollMessageId} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetActivePollMessageId(long userId)
        {
            string commandText = $"SELECT ActivePollMessageId FROM UserFlags WHERE UserID = {userId}";

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
    }
}
