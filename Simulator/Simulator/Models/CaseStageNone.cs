namespace Simulator.Models
{
    internal class CaseStageNone : CaseStage
    {
        public CaseStageNone(
            int number,
            string textBefore,
            string textAfter,
            int moduleNumber
            )
            : base(number, textBefore, textAfter, moduleNumber)
        {}
    }
}
