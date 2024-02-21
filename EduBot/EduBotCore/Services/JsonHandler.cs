using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Simulator.TelegramBotLibrary
{
    public static class JsonHandler
    {
        public static async Task WriteToJson(string filePath, JObject jsonObject)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                using (var sw = new StreamWriter(stream))
                {
                    string jsonText = jsonObject.ToString();
                    
                    await sw.WriteAsync(jsonText);
                    stream.SetLength(jsonText.Length);
                }
            }
        }

        public static async Task<JObject> ReadJsonFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(stream))
                {
                    string json = await sr.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<JObject>(json);
                }
            }
        }

        public static async Task CheckJsonFile(string filePath)
        {
            using (StreamWriter streamWriter = File.CreateText(filePath))
            {
                await streamWriter.WriteAsync("{}");
            }

        }
    }
}
