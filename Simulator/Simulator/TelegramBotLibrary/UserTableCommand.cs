using Simulator.BotControl.State;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Simulator.TelegramBotLibrary
{
    public static class UserTableCommand
    {
        private static SqlCommand command = new SqlCommand();

        static UserTableCommand()
        {
            command.Connection = LocalSqlDbConnection.Connection;
        }
   
        public static void Dispose()
        {
            command.Dispose();
        }

        public static void AddUser(User user)
        {
            string commandText = $"insert into Users (UserId, Name, Surname, IsAdmin, isGroupLeader, DialogState, GroupNumber)" +
                $" values ('{user.UserID}','{user.Name}','{user.Surname}','{false}','{false}','{0}','{user.GroupNumber}')";
            //Добавлять пользователя (если его нет в базе)
            ExecuteNonQueryCommand(commandText);
        }
        public static DialogState GetDialogState(long userId)
        {
            string commandText = $"select * from Users where UserId = {userId}";
            //Искать по уникальному идентификтаору, есть ли такой пользователь
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (DialogState)reader["DialogState"];
                }
            }
            return DialogState.None;
        }
        public static void SetDialogState(long userId, DialogState state)
        {
            string commandText = $"update Users set DialogState = '{(int)state}' where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        public static bool HasUser(long userId)
        {
            string commandText = $"select * from Users where UserId = {userId}";
            //Искать по уникальному идентификтаору, есть ли такой пользователь
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }
        public static bool IsAdmin(long userId)
        {
            string commandText = $"select * from Users where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (bool)reader["IsAdmin"];
                }
            }
            throw new ArgumentException("Not foubd user in database");
        }
        public static User GetUserById(long userId)
        {
            string commandText = $"select * from Users where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                GetUserFromReader(reader, out User user);
                return user;
            }
        }
        public static void SetAccess(long userId, bool up)
        {
            string commandText = $"update Users set IsAdmin = '{up}' where UserId = {userId}";
            //Назначить пользователя администратором
            ExecuteNonQueryCommand(commandText);
        }
        public static List<User> GetGroupUsers(string groupNumber)
        {
            command.CommandText = $"select * from Users where GroupNumber = '{groupNumber}'";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<User> registeredUsers = new List<User>();
                while (GetUserFromReader(reader, out User user))
                {
                    registeredUsers.Add(user);
                }
                return registeredUsers;
            }
        }
        public static string GetGroupNumber(long userId)
        {
            command.CommandText = $"select * from Users where UserId = {userId}";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if(reader.Read())
                {
                    return (string)reader["GroupNumber"];
                }
                return null;
            }
        }
        public static void DeleteUser(long userId)
        {
            string commandText = $"delete from Users where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        private static bool GetUserFromReader(SqlDataReader reader, out User user)
        {
            user = null;
            if (reader.Read())
            {
                user = new User((int)reader["UserID"], (string)reader["Name"], (string)reader["Surname"]);
                user.IsAdmin = (bool)reader["IsAdmin"];
                user.GroupNumber = (string)reader["GroupNumber"];
                return true;
            }
            return false;
        }

        private static void ExecuteNonQueryCommand(string commandText)
        {
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}
