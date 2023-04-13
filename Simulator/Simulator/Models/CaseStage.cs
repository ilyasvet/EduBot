using Simulator.Case;
using System;

namespace Simulator.Models
{
    internal class CaseStage
    {
        public int Number { get; private set; }
        public bool ConditionalMove { get; private set; }
        public string Text { get; private set; }
        public int NextStage { get; set; }
        public AnswerType AnswerType { get; private set; }
        public bool OneAnswer { get; set; }

        //Массив пар (номер варианта ответа - номер этапа для перехода)
        //Используется когда ConditionalMove = true

        public ValueTuple<int, int>[] MovingNumbers { get; set; }
        public CaseStage(int number, bool conditionalMove, string text, AnswerType answerType)
        {
            Number = number;
            ConditionalMove = conditionalMove;
            Text = text;
            AnswerType = answerType;
        }
    }
}
