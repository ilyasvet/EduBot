using System;
using System.Diagnostics;

namespace Simulator.Services
{
    public static class ControlSystem
    {
        public static object Monitor = new();
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
    }
}
