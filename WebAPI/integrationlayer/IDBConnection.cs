using System.Collections.Generic;

namespace ToDoList.integrationlayer
{
    public interface IDBConnection
    {
        public void AddNewItem(Item item);

        public void UpdateItem(Item item);

        public Item GetSingleItem(int id);

        public List<Item> GetAllItems();

        public void DeleteItem(int id);

        public void DeleteAllDoneItems();

        public void DeleteAllItems();
    }
}
