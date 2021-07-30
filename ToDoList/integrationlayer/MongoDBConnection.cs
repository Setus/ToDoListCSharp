using System;
using System.Collections.Generic;
using MongoDB.Bson;
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

        public void AddNewItem(Item item)
        {
            Console.WriteLine("Mongodb, called AddNewItem, with the new object: " + item.itemId + ", " + item.itemName + ", " + item.done);
            CreateNewConnection();
            itemsCollection.InsertOne(item);
        }

        public void UpdateItem(Item item)
        {
            Console.WriteLine("Mongodb, called UpdateItem()");
            CreateNewConnection();
            Item oldItem = GetSingleItem(item.itemId);
            if (oldItem == null)
            {
                throw new ArgumentNullException("Trying to update an item that does not exist");
            }
            item.Id = oldItem.Id;
            var result = itemsCollection.ReplaceOne(
                new BsonDocument("itemId", item.itemId),
                item,
                new ReplaceOptions { IsUpsert = false });
            Console.WriteLine(result);
        }


        public void DeleteItem(int id)
        {
            Console.WriteLine("Mongodb, called DeleteItem()");
            CreateNewConnection();
            itemsCollection.DeleteOne(item => item.itemId == id);
        }

        public void DeleteAllDoneItems()
        {
            Console.WriteLine("Mongodb, called DeleteAllDoneItems()");
            CreateNewConnection();
            itemsCollection.DeleteMany(item => item.done);
        }

        public void DeleteAllItems()
        {
            Console.WriteLine("Mongodb, called DeleteAllItems()");
            CreateNewConnection();
            itemsCollection.DeleteMany(item => true);
        }

        public List<Item> GetAllItems()
        {
            Console.WriteLine("Mongodb, called GetAllItems()");
            CreateNewConnection();

            List<Item> items = itemsCollection.Find(item => true).ToList();

            foreach (Item item in items)
            {
                Console.WriteLine(item.ToString());
            }
            return items;
        }

        public Item GetSingleItem(int id)
        {
            Console.WriteLine("Mongodb, called GetSingleItem()");
            CreateNewConnection();
            List<Item> items = itemsCollection.Find(item => item.itemId == id).Limit(1).ToList();
            if (items.Count == 0)
            {
                return null;
            }
            Console.WriteLine(items[0].ToString());
            return items[0];
        }
    }
}
