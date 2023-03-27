namespace Simulator.Models
{
    public class Group
    {
        public string GroupNumber { get; private set; }
        public string Password { get; set; }
        public Group(string number)
        {
            GroupNumber = number;
        }
    }
}
