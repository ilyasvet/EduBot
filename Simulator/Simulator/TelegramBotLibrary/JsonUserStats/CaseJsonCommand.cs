﻿using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using Simulator.Case;

namespace Simulator.TelegramBotLibrary
{
    public static class CaseJsonCommand
    {
        private static readonly string statsDirectory = 
            $"{AppDomain.CurrentDomain.BaseDirectory}" +
            $"{ConfigurationManager.AppSettings["PathStats"]}";
        public static async Task AddValueToJsonFile(long userId, (int, int) StageNotaskNo, StageResults results, int attemptNo)
        {
            string filePath = $"{statsDirectory}\\{userId}.json";

            int moduleNumber = StageNotaskNo.Item1;
            int stageNumber = StageNotaskNo.Item2;

            JObject jsonObject = await ReadJsonFile(filePath);

           
            
            string key = $"{moduleNumber}-{stageNumber}-{attemptNo}";

            // Записываем данные
            string rateKey = "rate-" + key;
            string answersKey = "answers-" + key;
            string timeKey = "time-" + key;

            jsonObject[rateKey] = results.Rate;
            jsonObject[answersKey] = results.Answers;
            jsonObject[timeKey] = results.Time;

            await WriteToJson(filePath, jsonObject);
        }
        public static async Task<object> GetValueFromJson(long userId, string key)
        {
            string filePath = $"{statsDirectory}\\{userId}.json";
            JObject jsonObject;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(stream))
                {
                    string json = await sr.ReadToEndAsync();
                    jsonObject = JsonConvert.DeserializeObject<JObject>(json);
                }
            }
            return jsonObject[key];
        }
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