using System;
using System.Collections.Generic;
using System.Configuration;
using ToDoList.integrationlayer;

namespace ToDoList
{
    public class ItemOperations
    {

        public DBConnectionInterface dBConnection;

        public ItemOperations(string databaseType)
        {
            if (databaseType.Equals("mongodb"))
            {
                dBConnection = MongoDBConnection.GetSingletonInstance();
            }
            else
            {
                dBConnection = MySQLDBConnection.GetSingletonInstance();
            }
        }

        public void AddNewItem(Item newItem)
        {
            Console.WriteLine("About to add the new object: " + newItem.ToString());
            dBConnection.AddNewItem(newItem.itemId, newItem.itemName, newItem.done ? 1 : 0);
        }

        public void UpdateItem(Item updatedItem)
        {
            Console.WriteLine("Called UpdateItem()");
            dBConnection.UpdateItem(updatedItem.itemId, updatedItem.itemName, updatedItem.done ? 1 : 0);
        }

        public void DeleteItem(Item deletedItem)
        {
            Console.WriteLine("Called DeleteItem()");
            dBConnection.DeleteItem(deletedItem.itemId);
        }

        public void DeleteAllDoneItems()
        {
            Console.WriteLine("Called DeleteAllDoneItems()");
            dBConnection.DeleteAllDoneItems();
        }

        public List<Item> GetAllItems()
        {
            Console.WriteLine("Called GetAllItems()");
            return dBConnection.GetAllItems();
        }

        public Item GetSingleItem(int itemId)
        {
            Console.WriteLine("Called GetSingleItem()");
            return dBConnection.GetSingleItem(itemId);
        }


    }
}