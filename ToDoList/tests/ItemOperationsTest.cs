using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace ToDoList
{

    [TestFixture]
    class ItemOperationsTest
    {
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
        public void TestGetAllItemsReturnsItemsInItemIdOrder()
        {
            TestGetAllItemsReturnsItemsInItemIdOrder("mysql");
            TestGetAllItemsReturnsItemsInItemIdOrder("mongodb");
        }

        [Test]
        public void TestDeleteAllDone()
        {
            TestDeleteAllDone("mysql");
            TestDeleteAllDone("mongodb");
        }

        [Test]
        public void TestDeleteAllItems()
        {
            TestDeleteAllItems("mysql");
            TestDeleteAllItems("mongodb");
        }

        public void TestCRUDOperations(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ItemOperations itemOperations = new(databasetype);
            Item testItem0 = new(100, "Test item", false);
            Item testItem1 = new(100, "Update item name", false);
            Item testItem2 = new(100, "Update item NAME and done", true);

            Assert.IsNull(itemOperations.GetSingleItem(testItem0.itemId));

            itemOperations.AddNewItem(testItem0);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem0.itemId).Equals(testItem0));

            itemOperations.UpdateItem(testItem1);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));

            itemOperations.UpdateItem(testItem2);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem2.itemId).Equals(testItem2));

            itemOperations.DeleteItem(testItem2);

            Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));
        }

        public void TestGetAllItems(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ItemOperations itemOperations = new(databasetype);

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

            Assert.IsTrue(itemOperations.GetSingleItem(testItem0.itemId).Equals(testItem0));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem2.itemId).Equals(testItem2));

            listOfItems = itemOperations.GetAllItems();
            foreach (Item item in listOfItems)
            {
                Console.WriteLine(item.ToString());
            }
            Assert.IsTrue(listOfItems[0].Equals(testItem0));
            Assert.IsTrue(listOfItems[1].Equals(testItem1));
            Assert.IsTrue(listOfItems[2].Equals(testItem2));

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
            ItemOperations itemOperations = new(databasetype);

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
            Assert.IsTrue(listOfItems[0].Equals(testItem0));
            Assert.IsTrue(listOfItems[1].Equals(testItem1));
            Assert.IsTrue(listOfItems[2].Equals(testItem2));

            itemOperations.DeleteItem(testItem0);
            itemOperations.DeleteItem(testItem1);
            itemOperations.DeleteItem(testItem2);

            listOfItems = itemOperations.GetAllItems();
            Assert.IsTrue(listOfItems.Count == 0);
        }

        public void TestDeleteAllDone(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ItemOperations itemOperations = new(databasetype);

            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(itemOperations.GetAllItems().Count == 0);

            Item testItem0 = new(100, "Item A", true);
            Item testItem1 = new(101, "Item B", false);
            Item testItem2 = new(102, "Item C", true);

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem0.itemId).Equals(testItem0));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem2.itemId).Equals(testItem2));

            itemOperations.DeleteAllDoneItems();

            Assert.IsTrue(itemOperations.GetAllItems().Count == 1);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));

            itemOperations.DeleteItem(testItem1);
            Assert.IsNull(itemOperations.GetSingleItem(testItem1.itemId));
        }

        public void TestDeleteAllItems(string databasetype)
        {
            Console.WriteLine("Runnig test with database type: " + databasetype);
            ItemOperations itemOperations = new(databasetype);

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
    }
}