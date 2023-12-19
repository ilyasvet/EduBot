using Simulator.Case;
using Simulator.Services;
using SimulatorCore.Models.CaseStage;

namespace SimulatorCore.Case
{
	internal class CoursesControl
	{
		public static CoursesList Courses { get; set; } = new CoursesList();

		public static void Make()
		{
			foreach (var courseDir in Directory.GetDirectories(ControlSystem.caseDirectory))
			{
				string path = courseDir + "\\" + ControlSystem.caseInfoFileName;

				if (File.Exists(path))
				{
					try
					{
						CaseConverter.FromFile(path);
					}
					catch
					{
						ControlSystem.DeleteFilesFromDirectory(courseDir);
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
}
