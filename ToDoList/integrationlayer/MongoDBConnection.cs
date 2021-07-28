using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace ToDoList.integrationlayer
{
    public class MongoDBConnection : DBConnectionInterface
    {

        private IMongoCollection<Item> itemsCollection = null;

        private static readonly MongoDBConnection _instance = new();

        public static MongoDBConnection GetSingletonInstance()
        {
            return _instance;
        }

        public void CreateNewConnection()
        {
            if (itemsCollection == null)
            {
                itemsCollection = new MongoClient()
                    .GetDatabase("ToDoListDB")
                    .GetCollection<Item>("items");
            }
        }

        public void AddNewItem(int id, string itemName, int done)
        {

        }

        public void UpdateItem(int id, string itemName, int done)
        {

        }


        public void DeleteItem(int id)
        {

        }

        public void DeleteAllDoneItems()
        {

        }

        public List<Item> GetAllItems()
        {
            Console.WriteLine("Calling mongodb GetAllItems");
            List<Item> items = itemsCollection.Find(item => true).ToList();

            foreach (Item item in items)
            {
                Console.WriteLine(item.ToString());
            }
            return null;
        }

        public Item GetSingleItem(int id)
        {
            CreateNewConnection();
            Console.WriteLine("Calling mongodb GetSingleItem");
            List<Item> items = itemsCollection.Find(item => item.itemId == id).Limit(1).ToList();
            Console.WriteLine(items[0].ToString());
            return items[0];
        }
    }
}
