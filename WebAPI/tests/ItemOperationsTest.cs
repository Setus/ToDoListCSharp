using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using ToDoList.integrationlayer;

namespace ToDoList
{

    [TestFixture]
    class ItemOperationsTest
    {
        string mysql = "mysql";
        string mongodb = "mongodb";
        List<IDBConnection> iDBConnections = null;

        [Test]
        public void TestCRUDOperations()
        {
            TestCRUDOperations(mysql);
            TestCRUDOperations(mongodb);
        }

        [Test]
        public void TestGetAllItems()
        {
            TestGetAllItems(mysql);
            TestGetAllItems(mongodb);
        }

        [Test]
        public void TestGetAllItemsReturnsItemsInItemIdOrder()
        {
            TestGetAllItemsReturnsItemsInItemIdOrder(mysql);
            TestGetAllItemsReturnsItemsInItemIdOrder(mongodb);
        }

        [Test]
        public void TestDeleteAllDone()
        {
            TestDeleteAllDone(mysql);
            TestDeleteAllDone(mongodb);
        }

        [Test]
        public void TestDeleteAllItems()
        {
            TestDeleteAllItems(mysql);
            TestDeleteAllItems(mongodb);
        }

        public void TestCRUDOperations(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());
            Item testItem0 = new(100, "Test item", false);
            Item testItem1 = new(100, "Update item name", false);
            Item testItem2 = new(100, "Update item NAME and done", true);

            Assert.IsNull(itemOperations.GetSingleItem(testItem0.itemId));

            itemOperations.AddNewItem(testItem0);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem0.itemId), testItem0);

            itemOperations.UpdateItem(testItem1);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem1.itemId), testItem1);

            itemOperations.UpdateItem(testItem2);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem2.itemId), testItem2);

            //itemOperations.DeleteItem(testItem2);

            //Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));
        }

        public void TestGetAllItems(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            List<Item> listOfItems = itemOperations.GetAllItems();
            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(listOfItems.Count == 0);

            Item testItem0 = new(100, "Item A", false);
            Item testItem1 = new(101, "Item B", false);
            Item testItem2 = new(102, "Item C", true);

            Assert.IsNull(itemOperations.GetSingleItem(testItem0.itemId));
            Assert.IsNull(itemOperations.GetSingleItem(testItem1.itemId));
            Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem0.itemId), testItem0);
            Assert.AreEqual(itemOperations.GetSingleItem(testItem1.itemId), testItem1);
            Assert.AreEqual(itemOperations.GetSingleItem(testItem2.itemId), testItem2);

            listOfItems = itemOperations.GetAllItems();
            foreach (Item item in listOfItems)
            {
                Console.WriteLine(item.ToString());
            }
            Assert.AreEqual(listOfItems[0], testItem0);
            Assert.AreEqual(listOfItems[1], testItem1);
            Assert.AreEqual(listOfItems[2], testItem2);

            itemOperations.DeleteItem(testItem0);
            itemOperations.DeleteItem(testItem1);
            itemOperations.DeleteItem(testItem2);

            Assert.IsNull(itemOperations.GetSingleItem(testItem0.itemId));
            Assert.IsNull(itemOperations.GetSingleItem(testItem1.itemId));
            Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));
        }

        public void TestGetAllItemsReturnsItemsInItemIdOrder(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            List<Item> listOfItems = itemOperations.GetAllItems();
            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(itemOperations.GetAllItems().Count == 0);

            Item testItem2 = new(102, "Item A", false);
            Item testItem0 = new(100, "Item B", false);
            Item testItem1 = new(101, "Item C", true);

            itemOperations.AddNewItem(testItem2);
            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);

            listOfItems = itemOperations.GetAllItems();
            foreach (Item item in listOfItems)
            {
                Console.WriteLine(item.ToString());
            }
            Assert.AreEqual(listOfItems[0], testItem0);
            Assert.AreEqual(listOfItems[1], testItem1);
            Assert.AreEqual(listOfItems[2], testItem2);

            itemOperations.DeleteItem(testItem0);
            itemOperations.DeleteItem(testItem1);
            itemOperations.DeleteItem(testItem2);

            listOfItems = itemOperations.GetAllItems();
            Assert.IsTrue(listOfItems.Count == 0);
        }

        public void TestDeleteAllDone(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(itemOperations.GetAllItems().Count == 0);

            Item testItem0 = new(100, "Item A", true);
            Item testItem1 = new(101, "Item B", false);
            Item testItem2 = new(102, "Item C", true);

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem0.itemId), testItem0);
            Assert.AreEqual(itemOperations.GetSingleItem(testItem1.itemId), testItem1);
            Assert.AreEqual(itemOperations.GetSingleItem(testItem2.itemId), testItem2);

            itemOperations.DeleteAllDoneItems();

            Assert.AreEqual(itemOperations.GetAllItems().Count, 1);

            Assert.AreEqual(itemOperations.GetSingleItem(testItem1.itemId), testItem1);

            itemOperations.DeleteItem(testItem1);
            Assert.IsNull(itemOperations.GetSingleItem(testItem1.itemId));
        }

        public void TestDeleteAllItems(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ConfigurationManager.AppSettings["DatabaseType"] = databasetype;

            ItemOperations itemOperations = new(GetIDBConnections());

            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(itemOperations.GetAllItems().Count == 0);

            Item testItem0 = new(100, "Item A", true);
            Item testItem1 = new(101, "Item B", false);
            Item testItem2 = new(102, "Item C", true);

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.IsTrue(itemOperations.GetAllItems().Count == 3);

            itemOperations.DeleteAllItems();

            Assert.IsTrue(itemOperations.GetAllItems().Count == 0);
        }

        public IEnumerable<IDBConnection> GetIDBConnections()
        {
            if (iDBConnections == null || iDBConnections.Count == 0) {
                iDBConnections = new();
                iDBConnections.Add(new MySQLDBConnection());
                iDBConnections.Add(new MongoDBConnection());
            }
            return iDBConnections;
        }
    }


}