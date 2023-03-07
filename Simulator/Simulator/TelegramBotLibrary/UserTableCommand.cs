using Simulator.BotControl;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TelegramBotLibrary
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
            string commandText = $"insert into Users (UserId, FirstName, Surname)" +
                $" values ('{user.UserID}','{user.Name}','{user.Surname}')";
            //Добавлять пользователя (если его нет в базе)
            ExecuteNonQueryCommand(commandText);
        }
        public static DialogState GetDialogState(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            //Искать по уникальному идентификтаору, есть ли такой пользователь
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                return (DialogState)reader[9];
            }
            return DialogState.None;
        }
        public static void SetDialogState(long userId, DialogState state)
        {
            string commandText = $"update users set DialogState = {(int)state} where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        public static bool HasUser(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            //Искать по уникальному идентификтаору, есть ли такой пользователь
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();
            return reader.HasRows;
        }
        public static bool IsAdmin(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                return (bool)reader[5];
            }
            throw new ArgumentException();
        }
        public static User GetUserById(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();
            GetUserFromReader(reader, out User user);
            return user;
        }
        public static void SetAccess(long userId, bool up)
        {
            string commandText = $"update users set IsAdmin = {up} where UserId = {userId}";
            //Назначить пользователя администратором
            ExecuteNonQueryCommand(commandText);
        }
        public static void SetPassword(long userId, string password)
        {
            string commandText = $"update users set Password = {password} where UserId = {userId}";
            //Назначить пароль пользователю
            ExecuteNonQueryCommand(commandText);
        }
        public static void SetOnline(long userId, bool online)
        {
            string commandText = $"update users set Password = {online} where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        public static List<User> GetRegisteredUsers()
        {
            string commandText = $"select * from users where password is not null";
            return GetListUsers(commandText);
        }
        public static List<User> GetOnlineUsers()
        {
            string commandText = $"select * from users where IsOnline is true";
            return GetListUsers(commandText);
        }
        private static List<User> GetListUsers(string commandText)
        {
            command.CommandText = commandText;
            SqlDataReader reader = command.ExecuteReader();
            List<User> registeredUsers = new List<User>();
            while (GetUserFromReader(reader, out User user))
            {
                registeredUsers.Add(user);
            }
            return registeredUsers;
        }
        private static bool GetUserFromReader(SqlDataReader reader, out User user)
        {
            user = null;
            if (reader.Read())
            {
                user = new User((int)reader[0], (long)reader[1], (string)reader[2], (string)reader[3]);
                user.SetPassword((string)reader[4]);
                user.IsAdmin = (bool)reader[5];
                user.IsOnline = (bool)reader[6];
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
