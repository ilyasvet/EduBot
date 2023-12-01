using System;
using System.Collections.Generic;

namespace Simulator.Models
{
    internal abstract class CaseStage : IComparable
    {
        public int Number { get; set; } 
        public string TextBefore { get; set; }
        public int NextStage { get; set; }
        public int ModuleNumber { get; set; }
        public Dictionary<string, List<string>> AdditionalInfoFiles { get; set; }
        public int CompareTo(object obj)
        {
            if(obj is CaseStage stage)
            {
                return Number.CompareTo(stage.Number);
            }
            throw new ArgumentException();
        }
        public void AddAdditionalFile(string fileName)
        {
            if (AdditionalInfoFiles == null)
            {
                AdditionalInfoFiles = new Dictionary<string, List<string>>();
            }
            string extension = fileName.Split('.')[1];
            switch (extension)
            {
                case "mp3":
                    AddKeyToAdditionalFiles("audios");
                    AdditionalInfoFiles["audios"].Add(fileName);
                    break;
                case "mp4":
                    AddKeyToAdditionalFiles("videos");
                    AdditionalInfoFiles["videos"].Add(fileName);
                    break;
                case "jpg":
                    AddKeyToAdditionalFiles("photos");
                    AdditionalInfoFiles["photos"].Add(fileName);
                    break;
                default:
                    AddKeyToAdditionalFiles("docs");
                    AdditionalInfoFiles["docs"].Add(fileName);
                    break;
            }
        }
        private void AddKeyToAdditionalFiles(string key)
        {
            if (!AdditionalInfoFiles.ContainsKey(key))
            {
                AdditionalInfoFiles.Add(key, new List<string>());
            }
        }
    }
}
