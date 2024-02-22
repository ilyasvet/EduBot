using Telegram.Bot.Types.Enums;

namespace EduBot.Models
{
    internal class CaseStageMessage : CaseStage
    {
        public double Rate { get; set; }
        public MessageType MessageTypeAnswer { get; set; }
    }
}
