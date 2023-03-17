using Simulator.Models;

namespace Simulator.TelegramBotLibrary
{
    internal interface IGroupTableCommand
    {
        string GetPassword(int groupId);
        void SetPassword(int groupId, string password);
        void AddGroup(Group group);
        Group GetGroup(int groupId);
        void DeleteGroup(int groupId);
    }
}
