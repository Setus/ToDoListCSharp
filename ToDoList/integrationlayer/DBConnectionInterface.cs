using System.Collections.Generic;

namespace ToDoList.integrationlayer
{
    public interface DBConnectionInterface
    {

        public void AddNewItem(Item item);

        public void UpdateItem(Item item);

        public void DeleteItem(int id);

        public void DeleteAllDoneItems();

        public List<Item> GetAllItems();

        public Item GetSingleItem(int id);
    }
}
