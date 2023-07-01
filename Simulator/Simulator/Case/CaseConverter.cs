using Newtonsoft.Json.Linq;
using Simulator.Models;
using Simulator.TelegramBotLibrary;

namespace Simulator.Case
{
    internal static class CaseConverter
    {
        public static async void FromFile(string path)
        {
            JObject jsonOnject = await CaseJsonCommand.ReadJsonFile(path);
            StagesControl.Stages = jsonOnject.ToObject<StageList>();
        }
    }
}
