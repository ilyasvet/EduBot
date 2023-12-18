using Simulator.Models;
using System.Collections;

namespace SimulatorCore.Models.CaseStage
{
	internal class CoursesList : IEnumerable<StageList>
	{
		private List<StageList> listCourses = new List<StageList>();
		public StageList this[string courseName]
		{
			get => listCourses.First(c => c.CourseName == courseName);
		}
		public bool AddCourse(StageList course)
		{
			if (listCourses.FirstOrDefault(c => c.CourseName == course.CourseName) == null)
			{
				listCourses.Add(course);
				return true;
			}
			return false;
		}

		public bool Contains(string courseName)
		{
			return listCourses.FirstOrDefault(c => c.CourseName == courseName) != null;
		}

		public IEnumerator<StageList> GetEnumerator()
		{
			return listCourses.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
