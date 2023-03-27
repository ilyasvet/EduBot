using Simulator.Models;
using System;
using System.Data.SqlClient;

namespace Simulator.TelegramBotLibrary
{
    public static class GroupTableCommand
    {
        private static SqlCommand command = new SqlCommand();
        
        static GroupTableCommand()
        {
            command.Connection = LocalSqlDbConnection.Connection;
        }
        
        public static void Dispose()
        {
            command.Dispose();
        }
        
        public static string GetPassword(int groupId)
        {
            string commandText = $"select * from groups where ID = {groupId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if(reader.Read())
                {
                    return (string)reader["Password"];
                }
                throw new ArgumentNullException();
            }
        }
        public static int GetGroupId(string groupNumber)
        {
            string commandText = $"select * from groups where GroupNumber = {groupNumber}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (int)reader["ID"];
                }
                throw new ArgumentNullException();
            }
        }

        public static void SetPassword(int groupId, string password)
        {
            string commandText = $"update Groups set Password = '{password}' where ID = '{groupId}'";
            ExecuteNonQueryCommand(commandText);
        }

        public static void AddGroup(Group group)
        {
            string commandText = $"insert into Groups (GroupNumber, Password)" +
                                 $" values ('{group.GroupNumber}','{group.Password}')";
            ExecuteNonQueryCommand(commandText);
        }

        public static Group GetGroup(int groupId)
        {
            string commandText = $"select * from groups where ID = {groupId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                GetGroupFromReader(reader, out Group group);
                return group;
            }
        }

        public static void DeleteGroup(int groupId)
        {
            string commandText = $"delete from Groups where ID = '{groupId}'";
            ExecuteNonQueryCommand(commandText);
        }
        
        private static bool GetGroupFromReader(SqlDataReader reader, out Group group)
        {
            group = null;
            if (reader.Read())
            {
                group = new Group((string)reader["GroupNumber"]);
                group.Password = (string)reader["Password"];
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