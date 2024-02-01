using SimulatorCore.Properties;
using System.Diagnostics;

namespace Simulator.Services
{
    public static class ControlSystem
    {
        public static readonly string tempDirectory = PropertiesSystem.DocumentsDir;
        public static readonly string caseDirectory = PropertiesSystem.PathCase;
        public static readonly string statsDirectory = PropertiesSystem.PathStats;
        public static readonly string messageAnswersDirectory = PropertiesSystem.PathAnswers;
        public static readonly string caseInfoFileName = PropertiesSystem.CaseInfoFileName;
        public static readonly string botToken = PropertiesSystem.BotToken;
        public static readonly string statsFileName = PropertiesSystem.StatsFileName;
       
        public static void CreateDirectories()
        {
            CreateDirectory(tempDirectory);
            CreateDirectory(caseDirectory);
            CreateDirectory(statsDirectory);
            CreateDirectory(messageAnswersDirectory);
            CreateDirectory(messageAnswersDirectory + "/videos/");
            CreateDirectory(messageAnswersDirectory + "/audios/");
            CreateDirectory(messageAnswersDirectory + "/texts/");
            CreateDirectory(messageAnswersDirectory + "/other/");
        }

        public static void DeleteFilesFromDirectory(string directory)
        {
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
        public static void KillProcess(string name)
        {
            GC.Collect();
            Process[] List;
            List = Process.GetProcessesByName(name);
            foreach (Process proc in List)
            {
                proc.Kill();
            }
        }

        public static void ShowExceptionConsole(Exception ex)
        {
            Console.WriteLine("================EXCEPTION==============");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.TargetSite);
            Console.WriteLine("=======================================");
        }

        public static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
