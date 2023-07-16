using DbBotLibrary;
using Simulator.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class GroupTableCommand : CommandTable
    {
        public async Task AddGroup(Group group)
        {
            string commandText = $"INSERT INTO Groups (GroupNumber, Password)" +
                                 $" VALUES ('{group.GroupNumber}','{group.Password}')";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<Group> GetGroup(string groupNumber)
        {
            string commandText = $"SELECT * FROM Groups WHERE GroupNumber = {groupNumber}";

            Group result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                GetGroupFromReader(reader, out Group group);
                return group;
            }) as Group;

            return result;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            string commandText = $"SELECT * FROM Groups";

            List<Group> result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                List<Group> groups = new();
                while (GetGroupFromReader(reader, out Group group))
                {
                    groups.Add(group);
                }
                return groups;
            }) as List<Group>;

            return result;
        }

        public async Task<bool> HasGroup(string groupNumber)
        {
            string commandText = $"SELECT COUNT(GroupNumber) FROM Groups WHERE GroupNumber = '{groupNumber}'";

            bool result = (bool)await ExecuteReaderCommand(commandText, (reader) =>
            {
                reader.Read();
                return (int)reader[0] != 0;
            });

            return result;
        }

        public async Task DeleteGroup(string groupNumber)
        {
            string commandText = $"DELETE FROM Groups WHERE GroupNumber = '{groupNumber}'";
            await ExecuteNonQueryCommand(commandText);
        }

        public async Task<string> GetPassword(string groupNumber)
        {
            string commandText = $"SELECT Password FROM Groups WHERE GroupNumber = '{groupNumber}'";

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
     
        private bool GetGroupFromReader(SqlDataReader reader, out Group group)
        {
            group = null;
            if (reader.Read())
            {
                group = new Group((string)reader[0])
                {
                    Password = (string)reader[1]
                };
                return true;
            }
            return false;
        }
    }
}