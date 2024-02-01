using Newtonsoft.Json.Linq;
using Simulator.Models;
using Simulator.TelegramBotLibrary;
using SimulatorCore.Case;

namespace Simulator.Case
{
    internal static class CaseConverter
    {
        public static async void FromFile(string path)
        {
            JObject jsonOnject = await JsonHandler.ReadJsonFile(path);
            var courseMaterials = jsonOnject.ToObject<StageList>();
			CoursesControl.Courses.AddCourse(courseMaterials);
        }
    }
}
