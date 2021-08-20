using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ToDoList.integrationlayer;
using ToDoList.servicelayer;

namespace ToDoList
{
    public class ItemOperations : IOperations
    {

        public IDBConnection dbConnection;

        public ItemOperations(IEnumerable<IDBConnection> dbConnections)
        {
            var connections = dbConnections.ToArray();
            if (ReadDatabaseSetting().Equals("mysql"))
            {
                dbConnection = connections[0];
            }
            else
            {
                dbConnection = connections[1];
            }
        }

        public void AddNewItem(Item newItem)
        {
            dbConnection.AddNewItem(newItem);
        }

        public void UpdateItem(Item updatedItem)
        {
            dbConnection.UpdateItem(updatedItem);
        }

        public Item GetSingleItem(int itemId)
        {
            return dbConnection.GetSingleItem(itemId);
        }

        public List<Item> GetAllItems()
        {
            return dbConnection.GetAllItems().OrderBy(item => item.itemId).ToList();
        }

        public void DeleteItem(Item deletedItem)
        {
            dbConnection.DeleteItem(deletedItem.itemId);
        }

        public void DeleteAllDoneItems()
        {
            dbConnection.DeleteAllDoneItems();
        }

        public void DeleteAllItems()
        {
            dbConnection.DeleteAllItems();
        }

        private string ReadDatabaseSetting()
        {
            string key = "DatabaseType";
            string property = null;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                foreach (var thing in appSettings)
                {
                    Console.WriteLine(thing);
                }
                Console.WriteLine(appSettings.Count);
                property = appSettings[key] ?? throw new ArgumentException("database type is missing in app.config");

                Console.WriteLine("The property is: " + property);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("database type must be defined");
            }
            return property;
        }
    }
}