using System;
using System.Collections.Generic;

namespace ToDoList.servicelayer
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
