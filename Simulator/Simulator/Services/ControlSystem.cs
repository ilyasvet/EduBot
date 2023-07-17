using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Simulator.Services
{
    public static class ControlSystem
    {
        public static readonly string tempDirectory = ConfigurationManager.AppSettings["DocumentsDir"];
        public static readonly string caseDirectory = ConfigurationManager.AppSettings["PathCase"];
        public static readonly string statsDirectory = ConfigurationManager.AppSettings["PathStats"];
        public static readonly string messageAnswersDirectory = ConfigurationManager.AppSettings["PathAnswers"];
        public static readonly string caseInfoFileName = ConfigurationManager.AppSettings["CaseInfoFileName"];
        public static readonly string botToken = ConfigurationManager.AppSettings["BotToken"];
        public static readonly string statsFileName = ConfigurationManager.AppSettings["StatsFileName"];
        public static readonly string serverName = ConfigurationManager.AppSettings["ServerName"];
        public static readonly string dataBaseName = ConfigurationManager.AppSettings["DataBaseName"];

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

        private static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
