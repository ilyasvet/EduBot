using System.Collections.Generic;
using System.Linq;

namespace Simulator.Models
{
    internal class StageList
    {
        public const int HEADER_PROPERTIES_COUNT = 5;
        public string CourseName { get; set; }
        public bool ReCreateStats { get; set; }
        public int AttemptCount { get; set; }
        public bool ExtraAttempt { get; set; }
        public bool DeletePollAfterAnswer { get; set; }
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

        public int GetLastStageIndex()
        {
            return StagesEnd.First(s => s.IsEndOfCase == true).Number;
        }
    }
}
