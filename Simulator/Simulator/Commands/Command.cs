namespace Simulator.Commands
{
  public abstract class Command
  {
    public abstract void Execute(CommandParameters commandParameters);
  }
}
