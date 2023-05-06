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
        public AdditionalInfo AdditionalInfoType { get; set; }
        public List<string> NamesAdditionalFiles { get; set; }

        public CaseStagePoll(int number,
            string textBefore
            )
            : base(number, textBefore)
        {
            Options = new List<string>();
            PossibleRate = new Dictionary<int, double>();
        }
    }
}
