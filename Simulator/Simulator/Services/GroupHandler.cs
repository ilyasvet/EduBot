using Simulator.BotControl;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simulator.Services
{
    public static class GroupHandler
    {
        public static async Task AddGroup(string groupNumber)
        {
            Models.Group group = new Models.Group(groupNumber);
            group.SetPassword();
            await DataBaseControl.GroupTableCommand.AddGroup(group);
        }
        public static string GetGroupNumberFromPath(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1).Split('.')[0];
        }
        public static bool IsCorrectGroupNumber(string groupNumber)
        {
            Regex regex = new Regex("^[0-9]{7}-[0-9]{5}$");
            return regex.IsMatch(groupNumber);
        }
    }
}
