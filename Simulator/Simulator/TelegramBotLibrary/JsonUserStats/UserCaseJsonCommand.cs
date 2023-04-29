using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Simulator.TelegramBotLibrary
{
    public static class UserCaseJsonCommand
    {
        public static async Task AddValueToJsonFile(long userId, string key, string value)
        {
            string fileName = $"{userId}.json";

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

                jsonObject[key] = value;

                using (StreamWriter streamWriter = File.CreateText(fileName))
                {
                    await streamWriter.WriteAsync(jsonObject.ToString());
                }
            }
            else
            {
                throw new FileNotFoundException($"File {fileName} not found");
            }
        }

        public static async Task CheckAndCreateJsonFile(long userId)
        {
            string fileName = $"{userId}.json";

            if (!File.Exists(fileName))
            {
                using (StreamWriter streamWriter = File.CreateText(fileName))
                {
                    await streamWriter.WriteAsync("{}");
                }
            }
        }
        //For Kirill example of calling methods

        /*
         * var program = new Program();
        long userId = 123456;
        string key = "name";
        string value = "Kirill gay";

        await program.CheckAndCreateJsonFile(userId);

        await program.AddValueToJsonFile(userId, key, value);

        string fileName = $"{userId}.json";
        string json = await File.ReadAllTextAsync(fileName);
        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

        Console.WriteLine($"File content: {jsonObject.ToString()}");
         */
    }
}
