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
			foreach (var course in Courses)
			{
				string directory = ControlSystem.caseDirectory +
				"\\" + course.CourseName;
				string path = directory + "\\" + ControlSystem.caseInfoFileName;

				if (File.Exists(path))
				{
					try
					{
						CaseConverter.FromFile(path);
					}
					catch
					{
						DeleteCaseFiles(directory);
						throw;
					}
				}
				else
				{
					throw new FileNotFoundException();
				}
			}
		}

		public static void DeleteCaseFiles(string directory)
		{
			ControlSystem.DeleteFilesFromDirectory(directory);
		}
	}
}
