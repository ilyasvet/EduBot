using Simulator.TelegramBotLibrary;

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
    }
}
