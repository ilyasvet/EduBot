using Telegram.Bot.Types.Enums;

namespace Simulator.Models
{
    internal class CaseStageText : CaseStage
    {
        public MessageType MessageTypeAnswer { get; set; }

        public CaseStageText(
            int number,
            string textBefore,
            string textAfter,
            bool isEndOfModule,
            int moduleNumber,
            bool isEndOfCase
            )
            : base(number, textBefore, textAfter, isEndOfModule, moduleNumber, isEndOfCase)
        {}
    }
}
