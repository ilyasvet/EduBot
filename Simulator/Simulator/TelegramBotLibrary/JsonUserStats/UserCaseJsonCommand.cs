using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;

namespace Simulator.TelegramBotLibrary
{
    public static class UserCaseJsonCommand
    {
        public static async Task AddValueToJsonFile(long userId, (int, int) StageNotaskNo, object value, int attemptNo)
        {
            string fileName = $"{userId}.json";

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

                string stageKey = StageNotaskNo.Item1.ToString();

                // проверяем, что юзер получил самое первоe задание кейса
                if (attemptNo == 1 && StageNotaskNo.Item2 == 1 &&
                    value is DateTime && !jsonObject.ContainsKey("firstLaunchTime")
                    )
                {
                    // value приходит как DateTime т к при отправке задания вызывается метод AddValueToJsonFile
                    // с аргументом value = DateTime
                    jsonObject.Add("firstLaunchTime", value.ToString());
                }

                // создаем отдельный абзац для статистики определенном этапе
                if (!jsonObject.ContainsKey(StageNotaskNo.Item1.ToString()))
                {
                    JObject newJsonObject = new JObject();
                    jsonObject.Add(stageKey, newJsonObject);
                }

                // ищем абзац с текущего этапа
                foreach (var stage in jsonObject)
                {
                    if (stage.Key == stageKey)
                    {
                        // формируем ключ, который будем искать в json строке
                        string keyToSeatchFor = GetKeyForJsonObject(StageNotaskNo, value);

                        bool foundKey = false;
                        foreach (var keyValuePair in (JObject)stage.Value)
                        {
                            if (keyValuePair.Key == keyToSeatchFor)
                            {
                                // нашли ключ - модифицируем старое value. Сепаратор - это |
                                string newValue = GetValueForJsonObject(keyValuePair.Value.ToObject<string>(), value, attemptNo);
                                ((JObject)stage.Value).Remove(keyValuePair.Key);
                                ((JObject)stage.Value).Add(keyToSeatchFor, newValue);
                                foundKey = true;
                                break;
                            }
                        }

                        // не нашли в строке ключ - создаем с нуля
                        if (!foundKey)
                        {
                            string newValue = GetValueForJsonObject(null, value, attemptNo);
                            ((JObject)stage.Value).Add(keyToSeatchFor, newValue);
                        }
                        break;
                    }
                }

                using (StreamWriter streamWriter = File.CreateText(fileName))
                {
                    await streamWriter.WriteAsync(jsonObject.ToString());
                }
            }
            else
            {
                throw new FileNotFoundException($"Error add to Json stats. File {fileName} not found");
            }
        }

        private static string GetValueForJsonObject(string oldValue, object newValue, int attemptNo)
        {
            // в жсон строке необходимо хранить время выполнения задания. При первой записи в жсон (временно) там будет храниться
            // время отправки задания юзеру, а при второй записи этой же попытки будет браться время старой записи,
            // вычитаться новое время (время получения ответа от юзера) и записываться уже само время выполнения (финально)
            if (oldValue != null && newValue is DateTime)
            {
                string[] oldValues = oldValue.Split('|');
                // если во второй попытке ничего не хранится - записываем время отправки задания
                if (oldValues[1] == "" && attemptNo == 2)
                {
                    return oldValue + $"{newValue}|";
                }
                // массив будет хранить поиндексно время за 1 и за 2 попытки. Массив идет с нуля, а attemptNo - номер попытки, который
                // идет с единицы. Вычитая единицу получаем индекс массива, соответствующий номеру попытки
                DateTime oldDate = DateTime.Parse(oldValues[attemptNo - 1]);
                double MinutesSpentOnTask = ((DateTime)newValue - oldDate).TotalMinutes;
                // формируем строку данных
                if (oldValues[1] == "")
                {
                    return $"{MinutesSpentOnTask}|";
                }
                if (attemptNo == 1)
                {
                    return $"{MinutesSpentOnTask}|{oldValues[1]}";
                }
                else
                {
                    return $"{oldValues[0]}|{MinutesSpentOnTask}";
                }
            }
            else
            {
                if (attemptNo == 2)
                {
                    return oldValue + $"{newValue}";
                }
                return oldValue + $"{newValue}|";
            }
        }

        // формируем вид ключа в зависимости от типа value.
        // (int - баллы, DateTime - время, string - выбранные варианты ответа)
        private static string GetKeyForJsonObject((int, int) StageNotaskNo, object value)
        {
            string performedKey = $"({StageNotaskNo.Item2})";

            if (value is double)
            {
                performedKey += "pts";
            }
            else if (value is DateTime)
            {
                performedKey += "time";
            }
            else if (value is string)
            {
                performedKey += "answers";
            }
            else
            {
                throw new ArgumentException();
            }

            return performedKey;
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
