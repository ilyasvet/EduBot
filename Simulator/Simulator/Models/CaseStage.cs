using System.Collections.Generic;

namespace Simulator.Models
{
    internal abstract class CaseStage
    {
        public int Number { get; private set; } 
        public string TextBefore { get; private set; }
        public string TextAfter { get; private set; }
        public int NextStage { get; set; }

        //Массив пар (номер варианта ответа - номер этапа для перехода)
        //Используется когда ConditionalMove = true

        public Dictionary<int, int> MovingNumbers { get; set; }
        public CaseStage(int number, string textBefore, string textAfter)
        {
            Number = number;
            TextBefore = textBefore;
            TextAfter = textAfter;
        }
    }
}
