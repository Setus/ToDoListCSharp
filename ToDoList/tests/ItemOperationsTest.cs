using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace ToDoList
{

    [TestFixture]
    class ItemOperationsTest
    {
        [Test]
        public void TestCRUDOperationsMySQL()
        {
            ItemOperations itemOperationsMySQL = new("mysql");
            RunCRUDOperationTests(itemOperationsMySQL);
        }

        [Test]
        public void TestCRUDOperationsMongoDB()
        {
            ItemOperations itemOperationsMongoDB = new("mongodb");
            RunCRUDOperationTests(itemOperationsMongoDB);
        }

        public void RunCRUDOperationTests(ItemOperations itemOperations)
        {
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

        [Test]
        public void TestGetAllItemsMySQL()
        {
            ItemOperations itemOperationsMySQL = new("mysql");
            TestGetAllItems(itemOperationsMySQL);
        }

        [Test]
        public void TestGetAllItemsMongoDB()
        {
            ItemOperations itemOperationsMongoDB = new("mongodb");
            TestGetAllItems(itemOperationsMongoDB);
        }

        public void TestGetAllItems(ItemOperations itemOperations)
        {
            List<Item> listOfItems = itemOperations.GetAllItems();
            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(listOfItems.Count == 0);

            Item testItem0 = new(100, "Test item", false);
            Item testItem1 = new(101, "Update item name", false);
            Item testItem2 = new(102, "Update item NAME and done", true);

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

        [Test]
        public void TestDeleteAllDoneMySQL()
        {
            ItemOperations itemOperationsMySQL = new("mysql");
            TestDeleteAllDone(itemOperationsMySQL);
        }

        [Test]
        public void TestDeleteAllDoneMongoDB()
        {
            ItemOperations itemOperationsMongoDB = new("mongodb");
            TestDeleteAllDone(itemOperationsMongoDB);
        }

        public void TestDeleteAllDone(ItemOperations itemOperations)
        {
            List<Item> listOfItems = itemOperations.GetAllItems();
            // Cannot run this test if there already exists items in the database.
            Assert.IsTrue(listOfItems.Count == 0);

            Item testItem0 = new(100, "Test item", true);
            Item testItem1 = new(101, "Update item name", false);
            Item testItem2 = new(102, "Update item NAME and done", true);

            Assert.IsNull(itemOperations.GetSingleItem(testItem0.itemId));
            Assert.IsNull(itemOperations.GetSingleItem(testItem1.itemId));
            Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));

            itemOperations.AddNewItem(testItem0);
            itemOperations.AddNewItem(testItem1);
            itemOperations.AddNewItem(testItem2);

            Assert.IsTrue(itemOperations.GetSingleItem(testItem0.itemId).Equals(testItem0));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem2.itemId).Equals(testItem2));

            itemOperations.DeleteAllDoneItems();

            listOfItems = itemOperations.GetAllItems();
            Assert.IsTrue(listOfItems.Count == 1);

            Assert.IsNull(itemOperations.GetSingleItem(testItem0.itemId));
            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));
            Assert.IsNull(itemOperations.GetSingleItem(testItem2.itemId));

            itemOperations.DeleteItem(testItem1);
            Assert.IsNull(itemOperations.GetSingleItem(testItem1.itemId));
        }

        [Test]
        public void Testing()
        {
            ItemOperations itemOperationsMySQL = new("mongodb");
            itemOperationsMySQL.DeleteItem(new Item(1, "asd", false));
        }
    }
}