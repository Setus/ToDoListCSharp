using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ToDoList;
using WebAPI.Controllers;

namespace WebAPI
{

    [TestFixture]
    class ItemControllerTest
    {

        [Test]
        public void TestGetAllItems()
        {
            TestGetAllItems("mysql");
            TestGetAllItems("mongodb");
        }

        public void TestGetAllItems(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;
            ItemOperations itemOperations = new(databasetype);

            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(itemOperations.GetAllItems().Count == 0);

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

            Assert.IsTrue(itemOperations.GetAllItems().Count == 3);

            ItemController itemController = new ItemController();
            string jsonOutput = itemController.Get();
            Console.WriteLine("JsonOutput: " + jsonOutput);

            List<Item> newListOfItems = JsonConvert.DeserializeObject<List<Item>>(jsonOutput);

            for (int i = 0; i < originalListOfItems.Count; i++)
            {
                Console.WriteLine(originalListOfItems[i].ToString());
                Console.WriteLine(newListOfItems[i].ToString());
                Assert.IsTrue(originalListOfItems[i].Equals(newListOfItems[i]));
            }

            itemOperations.DeleteAllItems();
        }

        public void TestCRUDOperations(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;
            ItemOperations itemOperations = new(databasetype);

            Item testItem0 = new(100, "Test item", false);
            Item testItem1 = new(100, "Update item name", false);
            Item testItem2 = new(100, "Update item NAME and done", true);

            ItemController itemController = new ItemController();

            itemController.Create((JObject) JsonConvert.SerializeObject(testItem0));




            Assert.IsTrue(itemOperations.GetSingleItem(testItem0.itemId).Equals(testItem0));

            itemOperations.UpdateItem(testItem1);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));

            itemOperations.UpdateItem(testItem2);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem2.itemId).Equals(testItem2));

            itemOperations.DeleteItem(testItem2);

            Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));
        }

        [Test]
        public void DeleteAll()
        {
            ItemOperations itemOperations = new("mongodb");
            itemOperations.DeleteAllItems();
        }

    }
}