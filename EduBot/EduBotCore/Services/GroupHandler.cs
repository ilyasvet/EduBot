using EduBot.BotControl;
using System.Text.RegularExpressions;

using DbGroup = EduBotCore.Models.DbModels.Group;

namespace EduBot.Services
{
    public static class GroupHandler
    {
        public static async Task<bool> AddGroup(string groupNumber)
        {
            bool hasGroup = true;
            if (await DataBaseControl.GetEntity<DbGroup>(groupNumber) == null)
            {
                hasGroup = false;
            }
            if (!hasGroup)
            {
                DbGroup group = new DbGroup() { GroupNumber = groupNumber };
                group.SetPassword();
                await DataBaseControl.AddEntity(group);
                return true;
            }
            return false;
        }
        public static bool IsCorrectGroupNumber(string groupNumber)
        {
            Regex regex = new Regex("^[0-9]{7}-[0-9]{5}$");
            return regex.IsMatch(groupNumber);
        }
    }
}
