using Telegram.Bot.Types.Enums;

namespace Simulator.Models
{
    internal class CaseStageMessage : CaseStage
    {
        public double Rate { get; set; }
        public MessageType MessageTypeAnswer { get; set; }
    }
}
