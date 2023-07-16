using DbBotLibrary;
using Simulator.BotControl.State;
using Simulator.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class UserTableCommand : CommandTable
    {
        public async Task AddUser(User user, UserType type)
        {
            string commandText = $"INSERT INTO Users (UserID, Name, Surname)" +
                $" VALUES ('{user.UserID}','{user.Name}','{user.Surname}')";
            await ExecuteNonQueryCommand(commandText);

            commandText = $"INSERT INTO UsersState (UserID, DialogState, UserType, LogedIn)" +
                $" VALUES ('{user.UserID}', 0, {(int)type}, 0)";
            await ExecuteNonQueryCommand(commandText);

            commandText = $"INSERT INTO UserCourseState (UserId, Point, Rate, OnCourse)" +
                $" VALUES ('{user.UserID}', 0, 0, 0)";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<bool> HasUser(long userId)
        {
            string commandText = $"SELECT COUNT(UserID) FROM Users WHERE UserID = {userId}";
            //Искать по уникальному идентификтаору, есть ли такой пользователь
            bool result = (bool)await ExecuteReaderCommand(commandText, (reader) =>
            {
                reader.Read();
                return (int)reader[0] != 0;
            });
            return result;
        }

        public async void DeleteUser(long userId)
        {
            string commandText = $"DELETE FROM Users WHERE UserId = {userId}";
            await ExecuteNonQueryCommand(commandText);
            commandText = $"DELETE FROM UsersState WHERE UserId = {userId}";
            await ExecuteNonQueryCommand(commandText);
            commandText = $"DELETE FROM UserCourseState WHERE UserId = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task SetDialogState(long userId, DialogState state)
        {
            string commandText = $"UPDATE UsersSate SET DialogState = {(int)state} WHERE UserID = {userId}";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<DialogState> GetDialogState(long userId)
        {
            string commandText = $"SELECT DialogState FROM UsersState WHERE UserID = {userId}";
            
            DialogState result = (DialogState)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (DialogState)reader[0];
                }
                return null;
            });

            return result;
        }
       
        public async Task<UserType> GetUserType(long userId)
        {
            string commandText = $"SELECT UserType FROM UsersState WHERE UserID = {userId}";

            UserType result = (UserType)await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (UserType)(int)reader[0];
                }
                return null;
            });

            return result;
        }

        public async Task<User> GetUserById(long userId)
        {
            string commandText = $"SELECT u.UserID, u.Name, u.Surname, u.GroupNumber us.UserType" +
                $" FROM Users u" +
                $" INNER JOIN UsersState us" +
                $" ON u.UserID = us.UserID" +
                $" WHERE u.UserID = {userId}";
            
            User result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                GetUserFromReader(reader, out User user);
                return user;
            }) as User;

            return result;
        }

        public async Task<bool> IsLogedIn(long userId)
        {
            string commandText = $"SELECT LogedIn FROM UsersState WHERE UserID = {userId}";
            
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

        public async Task<List<User>> GetGroupUsers(string groupNumber)
        {
            string commandText = $"SELECT u.UserID, u.Name, u.Surname, u.GroupNumber us.UserType" +
                $" FROM Users u" +
                $" INNER JOIN UsersState us" +
                $" ON u.UserID = us.UserID" +
                $" WHERE u.GroupNumber = '{groupNumber}'";

            List<User> result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                List<User> registeredUsers = new();
                while (GetUserFromReader(reader, out User user))
                {
                    registeredUsers.Add(user);
                }
                return registeredUsers;
            }) as List<User>;

            return result;
        }

        public async Task<string> GetGroupNumber(long userId)
        {
            string commandText = $"SELECT GroupNumber FROM Users WHERE UserId = {userId}";

            string result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                if (reader.Read())
                {
                    return (string)reader[0];
                }
                return null;
            }) as string;

            return result;
        }

        private bool GetUserFromReader(SqlDataReader reader, out User user)
        {
            user = null;
            if (reader.Read())
            {
                user = new User((int)reader[0], (string)reader[1], (string)reader[2]);
                user.GroupNumber = (string)reader[3];
                user.UserType = (UserType)(int)reader[4];
                return true;
            }
            return false;
        }
    }
}
