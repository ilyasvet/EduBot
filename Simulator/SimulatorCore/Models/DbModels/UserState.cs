using DbLibrary.Attributes;
using Simulator.BotControl.State;
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
            set
            {
                if (value < 0)
                    throw new ArgumentException(Resources.WrongFormatID);
                _userID = value;
            }
        }
        public int DialogState { get; set; }
        public int UserType { get; set; }
        public bool LogedIn { get; set; }

        public UserType GetUserType()
        {
            return (UserType)UserType;
        }
        public void SetUserType(UserType userType)
        {
            UserType = (int)userType;
        }

        public DialogState GetDialogState()
        {
            return (DialogState)DialogState;
        }
        public void SetDialogState(DialogState newState)
        {
            DialogState = (int)newState;
        }

    }
}
