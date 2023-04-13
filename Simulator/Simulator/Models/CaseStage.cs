using Simulator.Case;
using System;

namespace Simulator.Models
{
    internal abstract class CaseStage
    {
        public int Number { get; private set; }
        
        public string Text { get; private set; }
        public int NextStage { get; set; }

        //Массив пар (номер варианта ответа - номер этапа для перехода)
        //Используется когда ConditionalMove = true

        public ValueTuple<int, int>[] MovingNumbers { get; set; }
        public CaseStage(int number, bool conditionalMove, string text)
        {
            Number = number;
            Text = text;
        }
    }
}
