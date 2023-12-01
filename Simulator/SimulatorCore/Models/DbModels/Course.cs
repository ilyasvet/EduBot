using DbLibrary.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimulatorCore.Models.DbModels
{
    [Table("Courses")]
    [PrimaryKey("CourseName", typeof(string))]
    internal class Course
    {
        public string? CourseName { get; set; }
    }
}
