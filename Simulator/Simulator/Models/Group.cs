using System;

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
        public override string ToString()
        {
            return $"{GroupNumber} === {Password}";
        }
        public void SetPassword()
        {
            string password = "";
            int passwordLenght = 6;
            Random rnd = new Random();
            for (int i = 0; i < passwordLenght; i++)
            {
                password += (char)(rnd.Next(25) + 97);
            }
            Password = password;
        }
    }
}
