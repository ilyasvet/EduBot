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
            set
            {
                if (value < 0)
                    throw new ArgumentException(Resources.WrongFormatID);
                _userID = value;
            }
        }
        public int StartDialogId { get; set; }

        public string? CurrentCourse { get; set; }

        public int ActivePollMessageId { get; set; }
    }
}
