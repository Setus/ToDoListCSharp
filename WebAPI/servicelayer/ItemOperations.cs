using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.integrationlayer;
using WebAPI.servicelayer;
using WebAPI.modellayer;

namespace WebAPI
{
    public class ItemOperations : IOperations
    {

        public IDBConnection dbConnection;

        public ItemOperations(IEnumerable<IDBConnection> dbConnections)
        {
            var connections = dbConnections.ToArray();
            string databaseType = AppConfigUtil.ReadDatabaseSetting("DatabaseType", true);
            if (databaseType.Equals(DatabaseType.mysql.ToString()))
            {
                dbConnection = connections[0];
            }
            else if (databaseType.Equals(DatabaseType.mongodb.ToString()))
            {
                dbConnection = connections[1];
            }
            else if (databaseType.Equals(DatabaseType.azuresql.ToString()))
            {
                dbConnection = connections[2];
            }
             else
            {
                throw new ArgumentException("The DatabaseType inputed in the app.config file does not correspond to any permitted value");
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

    }
}