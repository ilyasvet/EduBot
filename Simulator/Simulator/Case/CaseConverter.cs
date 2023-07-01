using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simulator.Models;
using Simulator.TelegramBotLibrary;
using System.Collections.Generic;

namespace Simulator.Case
{
    internal static class CaseConverter
    {
        public static async void FromFile(string path)
        {
            JObject jsonOnject = await CaseJsonCommand.ReadJsonFile(path);
            StagesControl.Stages = new StageList() { Stages = new List<CaseStage>() };
            var a = jsonOnject.Value<StageList>("poll");
            foreach (var stage in jsonOnject.Values("poll"))
            {
                CaseStagePoll stagePoll = JsonConvert.DeserializeObject<CaseStagePoll>(stage.ToString());
                StagesControl.Stages.Stages.Add(stagePoll);    
            }
            foreach (var stage in jsonOnject.Values("none"))
            {
                CaseStageNone stageNone = JsonConvert.DeserializeObject<CaseStageNone>(stage.ToString());
                StagesControl.Stages.Stages.Add(stageNone);
            }
            foreach (var stage in jsonOnject.Values("end"))
            {
                CaseStageEndModule stageEndModule = JsonConvert.DeserializeObject<CaseStageEndModule>(stage.ToString());
                StagesControl.Stages.Stages.Add(stageEndModule);
            }
            foreach (var stage in jsonOnject.Values("message"))
            {
                CaseStageMessage stageMessage = JsonConvert.DeserializeObject<CaseStageMessage>(stage.ToString());
                StagesControl.Stages.Stages.Add(stageMessage);
            }
        }
    }
}
