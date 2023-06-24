using System.Collections.Generic;

namespace Simulator.Models
{
    internal class CaseStagePoll : CaseStage
    {
        public bool ManyAnswers { get; set; }
        public List<string> Options { get; set; }
        public bool ConditionalMove { get; set; }
        public Dictionary<int, int> MovingNumbers { get; set; }
        public Dictionary<int, double> PossibleRate { get; set; }
        public Dictionary<int, double> NonAnswers { get; set; }
        public bool WatchNonAnswer { get; set; }
        public int Limit { get; set; }
        public double Fine { get; set; }

        public CaseStagePoll()
        {
            Options = new List<string>();
            PossibleRate = new Dictionary<int, double>();
        }
    }
}
