using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Simulator.Commands.ExtensionFiles
{
    public enum Result
    {
        None,
        Text,
        Photo
    }
    public class CommandResult
    {
        public Result Result { get; set; }
        public string? Text { get; set; }
        public InputOnlineFile InputOnlineFile { get; set; }
    }
}
