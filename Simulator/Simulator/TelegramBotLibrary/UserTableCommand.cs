using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TelegramBotLibrary
{
    public class UserTableCommand : AbstractSqlCommand
    {
        public UserTableCommand(LocalSqlDbConnection localConnection) : base(localConnection) { }

        public void AddUser(User user)
        {
            string commandText = $"insert into Users (UserId, FirstName, Surname)" +
                $" values ('{user.UserID}','{user.Name}','{user.Surname}')";
            //Добавлять пользователя (если его нет в базе)
            ExecuteNonQueryCommand(commandText);
        }
        public bool HasUser(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            //Искать по уникальному идентификтаору, есть ли такой пользователь
            _command.CommandText = commandText;
            SqlDataReader reader = _command.ExecuteReader();
            return reader.HasRows;
        }
        public bool IsAdmin(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            _command.CommandText = commandText;
            SqlDataReader reader = _command.ExecuteReader();
            if(reader.Read())
            {
                return (bool)reader[5];
            }
            throw new ArgumentException();
        }
        public User GetUserById(long userId)
        {
            string commandText = $"select * from users where UserId = {userId}";
            _command.CommandText = commandText;
            SqlDataReader reader = _command.ExecuteReader();
            GetUserFromReader(reader, out User user);
            return user;
        }
        public void SetAccess(long userId, bool up)
        {
            string commandText = $"update users set IsAdmin = {up} where UserId = {userId}";
            //Назначить пользователя администратором
            ExecuteNonQueryCommand(commandText);
        }
        public void SetPassword(long userId, string password)
        {
            string commandText = $"update users set Password = {password} where UserId = {userId}";
            //Назначить пароль пользователю
            ExecuteNonQueryCommand(commandText);
        }
        public void SetOnline(long userId, bool online)
        {
            string commandText = $"update users set Password = {online} where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        public List<User> GetRegisteredUsers()
        {
            string commandText = $"select * from users where password is not null";
            return GetListUsers(commandText);
        }
        public List<User> GetOnlineUsers()
        {
            string commandText = $"select * from users where IsOnline is true";
            return GetListUsers(commandText);
        }
        private List<User> GetListUsers(string commandText)
        {
            _command.CommandText = commandText;
            SqlDataReader reader = _command.ExecuteReader();
            List<User> registeredUsers = new List<User>();
            while (GetUserFromReader(reader, out User user))
            {
                registeredUsers.Add(user);
            }
            return registeredUsers;
        }
        private bool GetUserFromReader(SqlDataReader reader, out User user)
        {
            user = null;
            if (reader.Read())
            {
                user = new User((long)reader[1], (string)reader[2], (string)reader[3]);
                user.SetPassword((string)reader[4]);
                user.IsAdmin = (bool)reader[5];
                user.IsOnline = (bool)reader[6];
                return true;
            }
            return false;
        }
    }
}
