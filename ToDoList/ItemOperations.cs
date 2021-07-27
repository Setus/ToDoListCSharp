using System;
using System.Collections.Generic;

namespace ToDoList
{
    public class ItemOperations
    {

        public DBConnection myConnection;

        public ItemOperations()
        {
            myConnection = DBConnection.GetSingletonInstance();
        }

        public void AddNewItem(Item newItem)
        {
            Console.WriteLine("About to add the new object: " + newItem.ToString());
            myConnection.AddNewItem(newItem.id, newItem.itemName, newItem.done ? 1 : 0);
        }

        public void UpdateItem(Item updatedItem)
        {
            myConnection.UpdateItem(updatedItem.id, updatedItem.itemName, updatedItem.done ? 1 : 0);
        }

        public void DeleteItem(Item deletedItem)
        {
            myConnection.DeleteItem(deletedItem.id);
        }

        public void DeleteAllDoneItems()
        {
            myConnection.DeleteAllDoneItems();
        }

        public List<Item> GetAllItems()
        {
            Console.WriteLine("Called GetAllItems()");
            return ItemMapper.MapToListOfItems(myConnection.GetAllItems());
        }

        public Item GetItem(int itemId)
        {
            Console.WriteLine("Called GetItem()");
            return ItemMapper.MapToItem(myConnection.GetItemWithId(itemId));
        }


    }
}