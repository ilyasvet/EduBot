using Simulator.Case;
using Simulator.Services;
using EduBotCore.Models.CaseStage;

namespace EduBotCore.Case
{
	internal class CoursesControl
	{
		public static CoursesList Courses { get; set; } = new CoursesList();

		public static async Task Make()
		{
			foreach (var courseDir in Directory.GetDirectories(ControlSystem.caseDirectory))
			{
				await MakeCourse(courseDir);
			}
		}
		public static async Task ReMake(string coursePath)
		{
			await MakeCourse(coursePath);
		}
		private static async Task MakeCourse(string coursePath)
		{
			string path = coursePath + "\\" + ControlSystem.caseInfoFileName;

			if (File.Exists(path))
			{
				try
				{
					var courseData = await CaseConverter.FromFile(path);
					Courses.AddCourse(courseData);
				}
				catch
				{
					ControlSystem.DeleteFilesFromDirectory(coursePath);
					throw;
				}
			}
			else
			{
				throw new FileNotFoundException();
			}
		}
	}
}
