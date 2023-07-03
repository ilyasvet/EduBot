using System.Collections.Generic;

namespace Simulator.Models
{
    public enum AdditionalInfoType
    {
       None,
       Photo,
       Document,
       Video,
       Audio,
    }

    public class AdditionalInfo
    {
        private List<string> namesPhotos = new List<string>();
        private List<string> namesDocuments = new List<string>();
        private List<string> namesVideos = new List<string>();
        private List<string> namesAudios = new List<string>();
        public void Add(string fileName)
        {
            string extension = fileName.Split('.')[1];
            switch (extension)
            {
                case "mp3":
                    namesAudios.Add(fileName);
                    break;
                case "mp4":
                    namesVideos.Add(fileName);
                    break;
                case "jpeg":
                    namesPhotos.Add(fileName);
                    break;
                default:
                    namesDocuments.Add(fileName);
                    break;
            }
        }
        public List<string> GetNamesPhotos() {  return namesPhotos; }
        public List<string> GetNamesDocuments() {  return namesDocuments; }
        public List<string> GetNamesAudios() { return namesAudios; }
        public List <string> GetNamesVideos() {  return namesVideos; }

    }
}
