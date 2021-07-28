using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace ToDoList
{

    [TestFixture]
    class ItemOperationsTest
    {

        public ItemOperations itemOperations = new("mongodb");


        [Test]
        public void TestCRUDOperations()
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
        public void TestGetAllItems()
        {
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

            List<Item> listOfItems = itemOperations.GetAllItems();
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
        public void TestMongoDB()
        {
            Item testItem1 = new(1, "Gnocchi", true);
            Assert.IsTrue(itemOperations.GetSingleItem(testItem1.itemId).Equals(testItem1));
        }


    }
}