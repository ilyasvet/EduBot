using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Simulator.Services
{
    public static class Checker
    {
        public static bool TextIsCommand(Dictionary<string, string> dict, string text)
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
        public static string GetCommandCallbackQueryParam(string data)
        {
            string[] parameters = data.Split('|');
            if (parameters.Length > 1)
            {
                return parameters[1];
            }
            return "";
        }
        public static bool IsCorrectGroupNumber(string groupNumber)
        {
            Regex regex = new Regex("^[0-9]{7}-[0-9]{5}$");
            return regex.IsMatch(groupNumber);
        }
        public static bool IsCorrectFileExtension(string path, FileType fileType)
        {
            string extension = path.Split('.').Last();
            switch (fileType)
            {
                case FileType.ExcelTable:
                    return extension.Contains("xls");
                default:
                    return false;
            }
        }
    }
}
