namespace Simulator.Models
{
    internal class CaseStageEndModule : CaseStage
    {
        private const int GRADATION_COUNT = 3;
        public bool IsEndOfCase { get; set; }
        public double[] Rates { get; set; }
        public string[] Texts { get; set; }
        public int ModuleNumber { get; set; }
        public CaseStageEndModule(
            int number,
            string textBefore,
            string textAfter,
            int moduleNumber
            )
            : base(number, textBefore, textAfter, moduleNumber)
        {
            Rates = new double[GRADATION_COUNT]; //3 градации по баллам.
            Texts = new string[GRADATION_COUNT];
        } 
    }
}
