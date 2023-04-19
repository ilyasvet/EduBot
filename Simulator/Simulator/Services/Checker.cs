﻿using System.Collections.Generic;
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
        
        public static bool IsCorrectFileExtension(string path, FileType fileType)
        {
            string extension = path.Split('.').Last();
            string fileName = path.Split('\\').Last();
            switch (fileType)
            {
                case FileType.ExcelTable:
                    return extension.Contains("xls");
                case FileType.Case:
                    return fileName == "caseinfo.case";
                default:
                    return false;
            }
        }
    }
}
