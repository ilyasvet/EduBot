using Simulator.Commands.ExtensionFiles;

namespace Simulator.Commands
{
  public abstract class Command
  {
    public abstract CommandResult Execute(CommandParameters commandParameters);
  }
}
