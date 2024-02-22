using DbLibrary.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduBotCore.Models.DbModels
{
    [Table("courses")]
    [PrimaryKey("CourseName", typeof(string))]
    internal class Course
    {
        public string? CourseName { get; set; }
    }
}
