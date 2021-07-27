using System;
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
            itemOperations = new();
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
    }
}
