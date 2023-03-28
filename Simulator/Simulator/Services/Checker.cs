using Simulator.Commands;
using System.Collections.Generic;

namespace Simulator.Services
{
    public static class Checker
    {
        public static bool TextIsCommand(Dictionary<string, Command> dict, string text)
        {
            if (text.StartsWith("/"))
            {
                if (dict.ContainsKey(text))
                {
                    return true;
                }
            }
            return false;
        }
        public static string GetParam(string data)
        {
            string[] parameters = data.Split('|');
            if (parameters.Length > 1)
            {
                return parameters[1];
            }
            return "";
        }
    }
}
