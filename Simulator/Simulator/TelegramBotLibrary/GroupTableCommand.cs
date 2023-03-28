using System;
using System.Collections.Generic;
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
        
        public static string GetPassword(string groupNumber)
        {
            string commandText = $"select * from groups where GroupNumber = '{groupNumber}'";
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
        public static bool HasGroup(string groupNumber)
        {
            string commandText = $"select * from groups where GroupNumber = '{groupNumber}'";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        public static void SetPassword(string groupNumber, string password)
        {
            string commandText = $"update Groups set Password = '{password}' where GroupNumber = '{groupNumber}'";
            ExecuteNonQueryCommand(commandText);
        }

        public static void AddGroup(Models.Group group)
        {
            string commandText = $"insert into Groups (GroupNumber, Password)" +
                                 $" values ('{group.GroupNumber}','{group.Password}')";
            ExecuteNonQueryCommand(commandText);
        }

        public static Models.Group GetGroup(string groupNumber)
        {
            string commandText = $"select * from groups where GroupNumber = {groupNumber}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                GetGroupFromReader(reader, out Models.Group group);
                return group;
            }
        }
        public static List<Models.Group> GetAllGroups()
        {
            string commandText = $"select * from groups";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<Models.Group> groups = new List<Models.Group>();
                while(GetGroupFromReader(reader, out Models.Group group))
                {
                    groups.Add(group);
                }
                return groups;
            }
        }

        public static void DeleteGroup(string groupNumber)
        {
            string commandText = $"delete from Groups where GroupNumber = '{groupNumber}'";
            ExecuteNonQueryCommand(commandText);
        }
        
        private static bool GetGroupFromReader(SqlDataReader reader, out Models.Group group)
        {
            group = null;
            if (reader.Read())
            {
                group = new Models.Group((string)reader["GroupNumber"]);
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