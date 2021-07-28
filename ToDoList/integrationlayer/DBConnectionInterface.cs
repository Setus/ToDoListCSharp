using System.Collections.Generic;

namespace ToDoList.integrationlayer
{
    public interface DBConnectionInterface
    {

        public void AddNewItem(int id, string itemName, int done);

        public void UpdateItem(int id, string itemName, int done);

        public void DeleteItem(int id);

        public void DeleteAllDoneItems();

        public List<Item> GetAllItems();

        public Item GetSingleItem(int id);
    }
}
