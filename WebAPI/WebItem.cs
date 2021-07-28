using System;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoList;

namespace WebAPI
{
    public class WebItem
    {

        ItemOperations itemOperations;

        public WebItem()
        {
            itemOperations = new(ReadSetting("DatabaseType"));
        }

        public void AddNewItem(JObject payload)
        {
            itemOperations.AddNewItem(JsonConvert.DeserializeObject<Item>(payload.ToString()));
        }

        public void UpdateItem(JObject payload)
        {
            itemOperations.UpdateItem(JsonConvert.DeserializeObject<Item>(payload.ToString()));
        }

        public void DeleteItem(JObject payload)
        {
            itemOperations.DeleteItem(JsonConvert.DeserializeObject<Item>(payload.ToString()));
        }

        public string GetAllItems()
        {
            return JsonConvert.SerializeObject(itemOperations.GetAllItems());
        }

        public void DeleteAllDone()
        {
            itemOperations.DeleteAllDoneItems();
        }

        private string ReadSetting(string key)
        {
            string property = null;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                foreach (var thing in appSettings)
                {
                    Console.WriteLine(thing);
                }
                Console.WriteLine(appSettings.Count);
                property = appSettings[key] ?? "Not Found";
                Console.WriteLine("The property is: " + property);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("database type must be defined");
            }
            return property;
        }
    }
}
