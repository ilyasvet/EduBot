using System.Collections.Generic;
using System.Linq;

namespace Simulator.Models
{
    internal class StageList
    {
        public List<CaseStagePoll> StagesPoll { get; set; } = new List<CaseStagePoll>();

        public List<CaseStageNone> StagesNone { get; set; } = new List<CaseStageNone>();

        public List<CaseStageEndModule> StagesEnd { get; set; } = new List<CaseStageEndModule>();

        public List<CaseStageMessage> StagesMessage { get; set; } = new List<CaseStageMessage>();
        public CaseStage this[int num]
        {
            get
            {
                CaseStage caseStage = null;
                caseStage = StagesPoll.FirstOrDefault(s => s.Number == num) as CaseStage
                    ?? StagesMessage.FirstOrDefault(s => s.Number == num) as CaseStage
                    ?? StagesEnd.FirstOrDefault(s => s.Number == num) as CaseStage
                    ?? StagesNone.FirstOrDefault(s => s.Number == num);
                return caseStage;
            }
        }
        public Dictionary<int, int> GetTaskCountDictionary()
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (CaseStagePoll poll in StagesPoll)
            {
                AddToDictionaryCountTask(result, poll.ModuleNumber);
            }
            foreach (CaseStageMessage message in StagesMessage)
            {
                AddToDictionaryCountTask(result, message.ModuleNumber);
            }
            return result;
        }
        public List<int> GetStageNumbers(int moduleNumber)
        {
            var result = new List<int>(StagesPoll.
                    Where(s => s.ModuleNumber == moduleNumber).
                    Select(s => s.Number).
                    ToList());
            result.AddRange(
                    StagesMessage.Where(s => s.ModuleNumber == moduleNumber).
                    Select(s => s.Number).
                    ToArray()
                );
            result.Sort();
            return result;
                    
        }
        private void AddToDictionaryCountTask(Dictionary<int, int> result, int moduleNumber)
        {
            if (result.ContainsKey(moduleNumber))
            {
                result[moduleNumber]++;
            }
            else
            {
                result.Add(moduleNumber, 1);
            }
        }
    }
}
