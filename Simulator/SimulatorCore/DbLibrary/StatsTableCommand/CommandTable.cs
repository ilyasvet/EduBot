using DbLibrary.DbInterfaces;
using MySqlConnector;

namespace EduBotCore.DbLibrary.StatsTableCommand
{
	public class CommandTable
	{
		protected async Task<int> ExecuteNonQueryCommand(string commandText)
		{
			using (var command = new Command())
			{
				return await command.ExecuteNonQuery(commandText);
			}
		}

		protected async Task<object> ExecuteReaderCommand(string commandText, Func<MySqlDataReader, object> action)
		{
			using (var command = new Command())
			{
				var reader = await command.ExecuteReader(commandText);
				return action(reader);
			}
		}
	}
}