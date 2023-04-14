namespace Simulator.Models
{
    internal class CaseStageNone : CaseStage
    {
        public CaseStageNone(
            int number,
            string textBefore,
            string textAfter,
            bool isEndOfModule,
            int moduleNumber,
            bool isEndOfCase
            )
            : base(number, textBefore, textAfter, isEndOfModule, moduleNumber, isEndOfCase)
        {}
    }
}
