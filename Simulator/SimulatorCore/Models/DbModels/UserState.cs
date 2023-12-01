using DbLibrary.Attributes;
using Simulator.Properties;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimulatorCore.Models.DbModels
{
    [Table("UsersState")]
    [PrimaryKey("UserID", typeof(long))]
    internal class UserState
    {
        private long _userID;
        public long UserID
        {
            get => _userID;
            private set
            {
                if (value < 0)
                    throw new ArgumentException(Resources.WrongFormatID);
                _userID = value;
            }
        }
        public int DialogState { get; private set; }
        public int UserType { get; private set; }
        public bool LogedIn { get; private set; }

        public UserType GetUserType()
        {
            return (UserType)UserType;

        }
    }
}
