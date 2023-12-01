using System.ComponentModel.DataAnnotations.Schema;

namespace SimulatorCore.Models.DbModels
{
    [Table("GroupsCourses")]
    internal class GroupCourse
    {
        public string? GroupNumber { get; private set; }
        public string? CourseName { get; private set; }

    }
}
