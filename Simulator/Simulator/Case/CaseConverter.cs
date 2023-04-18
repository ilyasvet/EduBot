using Simulator.Models;
using System.IO;

namespace Simulator.Case
{
    internal static class CaseConverter
    {
        public static void FromFile(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string[] stageStrings = reader.ReadToEnd().Split('$');
                    //Вопросы разделяются знаком $
                    foreach (string stageString in stageStrings)
                    {
                        StagesControl.Stages.Stages.Add(StringToStage(stageString));
                    }
                }
            }
        }
        public static CaseStage StringToStage(string stringStage)
        {
            CaseStage stage = null;

            return stage;
        }
    }
}
