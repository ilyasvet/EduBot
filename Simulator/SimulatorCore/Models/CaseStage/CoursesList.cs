using Simulator.Models;

namespace SimulatorCore.Models.CaseStage
{
	internal class CoursesList
	{
		private List<StageList> listCourses = new List<StageList>();
		public StageList this[string courseName]
		{
			get => listCourses.First(c => c.CourseName == courseName);
		}
	}
}
