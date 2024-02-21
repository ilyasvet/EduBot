namespace EduBotCore.DbLibrary.StatsTableCommand
{
    // Тут получаем информацию с таблиц со статистикой для передачи в excel
    internal class StatsBuilderCommand : CommandTable
    {
        public async Task<List<string>> GetColumnsName(string TableName)
        {
            string commandText = $"SELECT * FROM {TableName}";

            List<string> result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                var result = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i));
                }

                return result;
            }) as List<string>;

            return result;
        }

        public async Task<List<List<object>>> GetAllTable(string TableName, string groupNumber = "")
        {
            string commandText;
            if (groupNumber == "all")
            {
                commandText = $"SELECT * FROM {TableName}";
            }
            else
            {
                commandText = $"SELECT * FROM {TableName} s WHERE s.UserID IN " +
                    $"(SELECT UserID FROM Users u WHERE u.GroupNumber = '{groupNumber}')";
            }

            List<List<object>> result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                var result = new List<List<object>>();

                int i = 0;
                while (reader.Read())
                {
                    result.Add(new List<object>());
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        result[i].Add(reader[j]);
                    }
                    i++;
                }

                return result;
            }) as List<List<object>>;

            return result;
        }

        public async Task<Dictionary<long, string>> GetUsers(string groupNumber = "")
        {
            string commandText;
            if (groupNumber == "all")
            {
                commandText = $"SELECT UserID, Name, Surname FROM Users";
            }
            else
            {
                commandText = $"SELECT UserID, Name, Surname FROM Users WHERE GroupNumber = '{groupNumber}'";
            }

            Dictionary<long, string> result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                var result = new Dictionary<long, string>();
                while (reader.Read())
                {
                    string userInfo = $"{(string)reader["Name"]} {(string)reader["Surname"]}";
                    result.Add((int)reader["UserID"], userInfo);
                }

                return result;
            }) as Dictionary<long, string>;

            return result;
        }
    }
}
