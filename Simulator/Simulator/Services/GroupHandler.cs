using Simulator.TelegramBotLibrary;
using System.Text.RegularExpressions;

namespace Simulator.Services
{
    public static class GroupHandler
    {
        public static void AddGroup(string groupNumber)
        {
            Models.Group group = new Models.Group(groupNumber);
            group.SetPassword();
            GroupTableCommand.AddGroup(group);
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
