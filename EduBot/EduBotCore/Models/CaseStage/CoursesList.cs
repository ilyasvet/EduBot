using EduBot.Models;
using System.Collections;

namespace EduBotCore.Models.CaseStage
{
	internal class CoursesList : IEnumerable<StageList>
	{
		private List<StageList> listCourses = new List<StageList>();
		public StageList this[string courseName]
		{
			get => listCourses.First(c => c.CourseName == courseName);
		}
		public void AddCourse(StageList course)
		{
			var same = listCourses.FirstOrDefault(c => c.CourseName == course.CourseName);
			if (same != null)
			{
				listCourses.Remove(same);
			}
			listCourses.Add(course);
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
