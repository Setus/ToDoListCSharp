using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            Console.WriteLine("WILL BE ADDIN THE OBJECT: " + newItem.ToString());
            dBConnection.AddNewItem(newItem);
        }

        public void UpdateItem(Item updatedItem)
        {
            dBConnection.UpdateItem(updatedItem);
        }

        public void DeleteItem(Item deletedItem)
        {
            dBConnection.DeleteItem(deletedItem.itemId);
        }

        public void DeleteAllDoneItems()
        {
            dBConnection.DeleteAllDoneItems();
        }

        public List<Item> GetAllItems()
        {
            return dBConnection.GetAllItems().OrderBy(item => item.itemId).ToList();
        }

        public Item GetSingleItem(int itemId)
        {
            return dBConnection.GetSingleItem(itemId);
        }


    }
}