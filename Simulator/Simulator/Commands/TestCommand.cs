using Simulator.Commands.ExtensionFiles;

namespace Simulator.Commands
{
    public class TestCommand : Command
    {
        public override CommandResult Execute(CommandParameters commandParameters)
        {
            return new CommandResult() { Text = "hello", Result = Result.Text };
        }
    }
}
