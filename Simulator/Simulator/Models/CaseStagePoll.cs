using System.Collections.Generic;

namespace Simulator.Models
{
    internal class CaseStagePoll : CaseStage
    {
        public bool ManyAnswers { get; set; }
        public string[] Options { get; set; }
        public bool ConditionalMove { get; set; }

        public Dictionary<int, double> PossibleRate { get; set; }

        public CaseStagePoll(int number,
            string textBefore,
            string textAfter,
            int moduleNumber
            )
            : base(number, textBefore, textAfter, moduleNumber) {}
    }
}
