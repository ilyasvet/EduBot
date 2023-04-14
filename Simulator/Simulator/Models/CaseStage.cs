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
        public bool IsEndOfModule { get; private set; }
        public bool IsEndOfCase { get; private set; }

        //Словарь пар (номер варианта ответа - номер этапа для перехода)
        //Используется когда ConditionalMove = true
        public Dictionary<int, int> MovingNumbers { get; set; }
        public CaseStage(
            int number,
            string textBefore,
            string textAfter,
            bool isEndOfModule,
            int moduleNumber,
            bool isEndOfCase
            )
        {
            Number = number;
            TextBefore = textBefore;
            TextAfter = textAfter;
            IsEndOfModule = isEndOfModule;
            ModuleNumber = moduleNumber;
            IsEndOfCase = isEndOfCase;
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
