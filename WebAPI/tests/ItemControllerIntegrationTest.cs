using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ToDoList;
using ToDoList.integrationlayer;
using WebAPI.Controllers;

namespace WebAPI
{

    [TestFixture]
    class ItemControllerIntegrationTest
    {
        List<IDBConnection> iDBConnections = null;

        [Test]
        public void TestCRUDOperations()
        {
            TestCRUDOperations("mysql");
            TestCRUDOperations("mongodb");
        }

        [Test]
        public void TestGetAllItems()
        {
            TestGetAllItems("mysql");
            TestGetAllItems("mongodb");
        }

        [Test]
        public void TestDeleteAllDone()
        {
            TestDeleteAllDone("mysql");
            TestDeleteAllDone("mongodb");
        }

        public void TestCRUDOperations(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            Assert.AreEqual(itemOperations.GetAllItems().Count, 0);

            Item testItem0 = new(100, "Test item", false);
            Item testItem1 = new(100, "Update item name", false);
            Item testItem2 = new(100, "Update item NAME and done", true);

            ItemController itemController = new ItemController(itemOperations);

            itemController.Create(JObject.Parse(JsonConvert.SerializeObject(testItem0)));

            string jsonOutput = itemController.GetSingle(testItem0.itemId);

            Item returnedItem = JsonConvert.DeserializeObject<Item>(jsonOutput);

            Assert.AreEqual(testItem0, returnedItem);

            itemController.Update(JObject.Parse(JsonConvert.SerializeObject(testItem1)));

            jsonOutput = itemController.GetSingle(testItem1.itemId);
            returnedItem = JsonConvert.DeserializeObject<Item>(jsonOutput);
            Assert.AreEqual(testItem1, returnedItem);

            Assert.AreEqual(itemOperations.GetAllItems().Count, 1);

            itemController.Update(JObject.Parse(JsonConvert.SerializeObject(testItem2)));

            jsonOutput = itemController.GetSingle(testItem2.itemId);
            returnedItem = JsonConvert.DeserializeObject<Item>(jsonOutput);
            Assert.AreEqual(testItem2, returnedItem);

            Assert.AreEqual(itemOperations.GetAllItems().Count, 1);

            itemController.Delete(JObject.Parse(JsonConvert.SerializeObject(testItem2)));

            jsonOutput = itemController.GetSingle(testItem2.itemId);
            returnedItem = JsonConvert.DeserializeObject<Item>(jsonOutput);
            Assert.IsNull(returnedItem);

            Assert.AreEqual(itemOperations.GetAllItems().Count, 0);
        }

        public void TestGetAllItems(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            // Cannot run this test if there already exists items in the database.
            Assert.AreEqual(itemOperations.GetAllItems().Count, 0);

            Item testItem0 = new(100, "Item A", false);
            Item testItem1 = new(101, "Item B", true);
            Item testItem2 = new(102, "Item C", false);

            List<Item> originalListOfItems = new List<Item>();
            originalListOfItems.Add(testItem0);
            originalListOfItems.Add(testItem1);
            originalListOfItems.Add(testItem2);

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.AreEqual(itemOperations.GetAllItems().Count, 3);

            ItemController itemController = new ItemController(itemOperations);
            string jsonOutput = itemController.GetAll();
            Console.WriteLine("JsonOutput: " + jsonOutput);

            List<Item> newListOfItems = JsonConvert.DeserializeObject<List<Item>>(jsonOutput);

            for (int i = 0; i < originalListOfItems.Count; i++)
            {
                Console.WriteLine(originalListOfItems[i].ToString());
                Console.WriteLine(newListOfItems[i].ToString());
                Assert.AreEqual(originalListOfItems[i], newListOfItems[i]);
            }

            itemOperations.DeleteAllItems();
        }

        public void TestDeleteAllDone(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            // Cannot run this test if there already exists items in the database.
            Assert.AreEqual(itemOperations.GetAllItems().Count, 0);

            Item testItem0 = new(100, "Item A", true);
            Item testItem1 = new(101, "Item B", false);
            Item testItem2 = new(102, "Item C", true);

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.AreEqual(itemOperations.GetAllItems().Count, 3);

            ItemController itemController = new ItemController(itemOperations);
            itemController.DeleteAllDone();

            Assert.AreEqual(itemOperations.GetAllItems().Count, 1);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem1.itemId), testItem1);

            itemOperations.DeleteAllItems();
        }

        public IEnumerable<IDBConnection> GetIDBConnections()
        {
            if (iDBConnections == null || iDBConnections.Count == 0)
            {
                iDBConnections = new();
                iDBConnections.Add(new MySQLDBConnection());
                iDBConnections.Add(new MongoDBConnection());
            }
            return iDBConnections;
        }
    }
}