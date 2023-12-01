using DbLibrary.Attributes;
using Simulator.Properties;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimulatorCore.Models.DbModels
{
    [Table("UserFlags")]
    [PrimaryKey("UserID", typeof(long))]
    internal class UserFlags
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
        public int StartDialogId { get; private set; }

        public bool OnCourse { get; private set; }

        public int ActivePollMessageId { get; private set; }
    }
}
