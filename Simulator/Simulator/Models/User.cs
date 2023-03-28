namespace Simulator.Models
{
    public class User
    {  
        public long UserID { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string GroupNumber { get; set; }
        public bool IsAdmin { get; set; }
        public object FirstAttempt { get; set; }
        public object SecondAttempt { get; set; }

        public User(long userId, string name, string surname)
        {
            UserID = userId;
            Name = name;
            Surname = surname;
            IsAdmin = false;
        }
        public override string ToString()
        {
            string result = Name + " " + Surname;
            if(IsAdmin)
            {
                result += " (Администратор)";
            }
            return result;
        }
    }
}
