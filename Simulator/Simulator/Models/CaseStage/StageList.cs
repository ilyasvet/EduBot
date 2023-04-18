using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.Models
{
    internal class StageList
    {
        public List<CaseStage> Stages { get; set; }
        public CaseStage this[int num]
        {
            get { return Stages.FirstOrDefault(s=>s.Number==num); }
        }
    }
}
