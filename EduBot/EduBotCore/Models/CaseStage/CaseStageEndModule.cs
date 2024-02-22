using System.Collections.Generic;

namespace EduBot.Models
{
    internal class CaseStageEndModule : CaseStage
    {
        public bool IsEndOfCase { get; set; }
        public List<double> Rates { get; set; }
        public List<string> Texts { get; set; }
        
        public CaseStageEndModule()
        {
            Rates = new List<double>();
        }
    }
}
