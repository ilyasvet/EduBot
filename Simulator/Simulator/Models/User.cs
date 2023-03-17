namespace Simulator.Models
{
    public class User
    {  
        public long UserID { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public int GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public object FirstAttempt { get; set; }
        public object SecondAttempt { get; set; }

        public User(long userId, string name, string surname)
        {
            UserID = userId;
            Name = name;
            Surname = surname;
        }
        public override string ToString() { return Name + " " + Surname; }
    }
}
