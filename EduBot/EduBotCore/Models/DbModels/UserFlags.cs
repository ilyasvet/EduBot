using DbLibrary.Attributes;
using EduBotCore.Properties;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduBotCore.Models.DbModels
{
    [Table("userflags")]
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
        public int StartDialogId { get; set; } = 0;

        public string? CurrentCourse { get; set; } = null;

        public int ActivePollMessageId { get; set; } = 0;
    }
}
