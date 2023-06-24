using System;
using System.Collections.Generic;

namespace Simulator.Models
{
    internal abstract class CaseStage : IComparable
    {
        public int Number { get; set; } 
        public string TextBefore { get; set; }
        public int NextStage { get; set; }
        public int ModuleNumber { get; set; }
        public AdditionalInfo AdditionalInfoType { get; set; }
        public List<string> NamesAdditionalFiles { get; set; }
        public int CompareTo(object obj)
        {
            if(obj is CaseStage stage)
            {
                return Number.CompareTo(stage.Number);
            }
            throw new ArgumentException();
        }
    }
}
