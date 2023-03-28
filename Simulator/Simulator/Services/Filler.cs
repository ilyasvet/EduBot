using Simulator.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Simulator.Services
{
    public static class Filler
    {
        public static Dictionary<string, Command> FillCommandDictionary()
        {
            Dictionary<string, Command> result = new Dictionary<string, Command>();

            Type baseType = typeof(Command);
            IEnumerable<Type> listOfSubclasses = Assembly.GetAssembly(baseType)
                .GetTypes()
                .Where(type => type.IsSubclassOf(baseType));

            foreach (Type type in listOfSubclasses)
            {
                Command commandObject = Activator.CreateInstance(type) as Command;
                result.Add(type.Name, commandObject);
            }
            return result;
        }
        public static Dictionary<string, string> FillAccordanceDictionary(string ResourceName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
            path += $"\\Properties\\{ResourceName}";
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] pair = line.Split(' ');
                        result.Add(pair[0], pair[1]);
                        line = sr.ReadLine();
                    }
                }
            }
            return result;
        }
    }
}
