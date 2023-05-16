using System;
using System.Collections.Generic;

namespace Simulator.Models
{
    internal abstract class CaseStage : IComparable
    {
        public int Number { get; private set; } 
        public string TextBefore { get; private set; }
        public int NextStage { get; set; }
        public int ModuleNumber { get; set; }
        public AdditionalInfo AdditionalInfoType { get; set; }
        public List<string> NamesAdditionalFiles { get; set; }
        public CaseStage(
            int number,
            string textBefore
            )
        {
            Number = number;
            TextBefore = textBefore;
        }

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
