using DbBotLibrary;
using System.Threading.Tasks;

namespace Simulator.TelegramBotLibrary
{
    public class CourseTableCommand : CommandTable
    {
        public async Task<bool> AddCourse(string courseName)
        {
            if (await HasCourse(courseName))
            {
                return false;
            }
            else
            {
                string commandText = $"INSERT INTO Courses (CourseName)" +
                    $" VALUES ('{courseName}')";
                await ExecuteNonQueryCommand(commandText);
                return true;
            }
        }

        private async Task<bool> HasCourse(string courseName)
        {
            string commandText = $"SELECT COUNT(CourseName) FROM Courses WHERE CourseName = {courseName}";
            bool result = (bool)await ExecuteReaderCommand(commandText, (reader) =>
            {
                reader.Read();
                return (int)reader[0] != 0;
            });
            return result;
        }
    }
}
