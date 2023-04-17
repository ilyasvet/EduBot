using System;
using System.Collections.Generic;

namespace Simulator.Models
{
    internal abstract class CaseStage : IComparable
    {
        public int Number { get; private set; } 
        public string TextBefore { get; private set; }
        public string TextAfter { get; private set; }
        public int NextStage { get; set; }
        public int ModuleNumber { get; private set; }
        //Словарь пар (номер варианта ответа - номер этапа для перехода)
        //Используется когда ConditionalMove = true
        public Dictionary<int, int> MovingNumbers { get; set; }
        public CaseStage(
            int number,
            string textBefore,
            string textAfter,
            int moduleNumber
            )
        {
            Number = number;
            TextBefore = textBefore;
            TextAfter = textAfter;
            ModuleNumber = moduleNumber;
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
