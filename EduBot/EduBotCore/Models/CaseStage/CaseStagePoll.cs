using System.Collections.Generic;

namespace Simulator.Models
{
    internal class CaseStagePoll : CaseStage
    {
        // Множественный выбор или нет?
        public bool ManyAnswers { get; set; }
        
        // Варианты ответов
        public List<string> Options { get; set; }

        // Переход на следующий стейдж зависит от ответа или нет?
        public bool ConditionalMove { get; set; }

        // Только при одиночном ответе! Словарь условных переходов. Номер ответа - номер стейджа, на который переходим
        public Dictionary<int, int> MovingNumbers { get; set; }

        // Количество баллов за ответ. Номер ответа - количество баллов
        public Dictionary<int, double> PossibleRate { get; set; }

        // Минус баллов, если ответ не указан. При множественном выборе. Номер ответа - штраф по баллам
        public Dictionary<int, double> NonAnswers { get; set; }

        // Нужно ли заполнять предыдущий список?
        public bool WatchNonAnswer { get; set; }

        // Лимит ответов (при множественном выборе)
        public int Limit { get; set; }

        // Штраф баллов за каждый ответ при превышенном лимите (при множественном выборе)
        public double Fine { get; set; }

        public CaseStagePoll()
        {
            NonAnswers = new Dictionary<int, double>();
            MovingNumbers = new Dictionary<int, int>();
            PossibleRate = new Dictionary<int, double>();
        }
    }
}
