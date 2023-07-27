using DbBotLibrary;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class UserFlagsTableCommand : CommandTable
    {
        private const string TABLE_NAME = "UserFlags";

        public async Task SetMessageStartDialogId(long userId, int messageId)
        {
            string commandText = $"UPDATE {TABLE_NAME} SET StartDialogId = {messageId} " +
                $"WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetMessageStartDialogId(long userId)
        {
            string commandText = $"SELECT StartDialogId FROM {TABLE_NAME} " +
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

        public async Task SetOnCourse(long userId, bool boolOnCourse)
        {
            string commandText = $"UPDATE {TABLE_NAME} SET OnCourse = '{boolOnCourse}' " +
                $"WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<bool> IsOnCourse(long userId)
        {
            string commandText = $"SELECT OnCourse FROM {TABLE_NAME} " +
                $"WHERE UserID = {userId}";

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

        public async Task SetActivePollMessageId(long userId, int activePollMessageId)
        {
            string commandText = $"UPDATE {TABLE_NAME} SET ActivePollMessageId = {activePollMessageId} " +
                $"WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<int> GetActivePollMessageId(long userId)
        {
            string commandText = $"SELECT ActivePollMessageId FROM {TABLE_NAME} " +
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
    }
}
