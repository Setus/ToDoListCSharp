using System.Collections.Generic;
using System.Linq;
using ToDoList.integrationlayer;
using ToDoList.servicelayer;
using WebAPI;

namespace ToDoList
{
    public class ItemOperations : IOperations
    {

        public IDBConnection dbConnection;

        public ItemOperations(IEnumerable<IDBConnection> dbConnections)
        {
            var connections = dbConnections.ToArray();
            if (AppConfigUtil.ReadDatabaseSetting("DatabaseType", true).Equals("mysql"))
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

    }
}