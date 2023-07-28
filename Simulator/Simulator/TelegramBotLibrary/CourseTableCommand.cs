using DbBotLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class CourseTableCommand : CommandTable
    {
        private const string TABLE_NAME = "Courses";
        public async Task<bool> AddCourse(string courseName)
        {
            if (await HasCourse(courseName))
            {
                return false;
            }
            else
            {
                string commandText = $"INSERT INTO {TABLE_NAME} (CourseName)" +
                    $" VALUES ('{courseName}')";
                await ExecuteNonQueryCommand(commandText);
                return true;
            }
        }

        public async Task<List<string>> GetListCourses()
        {
            string commandText = $"SELECT CourseName FROM {TABLE_NAME}";
            List<string> result = await ExecuteReaderCommand(commandText, (reader) =>
            {
                var result = new List<string>();
                while (reader.Read())
                {
                    result.Add((string)reader[0]);
                }
                return result;
            }) as List<string>;
            return result;
        }

        private async Task<bool> HasCourse(string courseName)
        {
            string commandText = $"SELECT COUNT(CourseName) FROM {TABLE_NAME} WHERE CourseName = {courseName}";
            bool result = (bool)await ExecuteReaderCommand(commandText, (reader) =>
            {
                reader.Read();
                return (int)reader[0] != 0;
            });
            return result;
        }
    }
}
