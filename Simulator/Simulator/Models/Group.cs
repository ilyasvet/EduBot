namespace Simulator.Models
{
    public class Group
    {
        public int GroupNumber { get; private set; }
        public string Password { get; set; }
        public Group(int number)
        {
            GroupNumber = number;
        }
    }
}
