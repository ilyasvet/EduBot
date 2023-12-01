using DbLibrary.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimulatorCore.Models.DbModels
{
    [Table("Groups")]
    [PrimaryKey("GroupNumber", typeof(string))]
    public class Group
    {
        public string? GroupNumber { get; private set; }
        public string? Password { get; private set; }
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
