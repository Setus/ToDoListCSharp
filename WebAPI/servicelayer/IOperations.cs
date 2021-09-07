using System.Collections.Generic;

namespace WebAPI.servicelayer
{
    public interface IOperations
    {
        void AddNewItem(Item newItem);

        void UpdateItem(Item updatedItem);

        Item GetSingleItem(int itemId);

        List<Item> GetAllItems();

        void DeleteItem(Item deletedItem);

        void DeleteAllDoneItems();

        void DeleteAllItems();
    }
}
