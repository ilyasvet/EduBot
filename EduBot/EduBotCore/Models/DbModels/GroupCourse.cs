using System.ComponentModel.DataAnnotations.Schema;

namespace EduBotCore.Models.DbModels
{
    [Table("groupscourses")]
    internal class GroupCourse
    {
        public string? GroupNumber { get; set; }
        public string? CourseName { get; set; }

    }
}
