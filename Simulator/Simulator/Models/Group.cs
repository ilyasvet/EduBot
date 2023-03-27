namespace Simulator.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupNumber { get; private set; }
        public string Password { get; set; }
        public Group(string number)
        {
            GroupNumber = number;
        }
        public override string ToString()
        {
            return $"{Id} === {GroupNumber} === {Password}";
        }
    }
}
