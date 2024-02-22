using Newtonsoft.Json.Linq;
using EduBot.Models;
using EduBot.TelegramBotLibrary;

namespace EduBot.Case
{
    internal static class CaseConverter
    {
        public static async Task<StageList> FromFile(string path)
        {
            JObject jsonOnject = await JsonHandler.ReadJsonFile(path);
            var courseMaterials = jsonOnject.ToObject<StageList>();
            return courseMaterials;
        }
    }
}
