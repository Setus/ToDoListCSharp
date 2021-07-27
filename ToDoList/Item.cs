using System;

namespace ToDoList
{
    public class Item
    {

        public int id { get; set; }
        public string itemName { get; set; }
        public Boolean done { get; set; }

        public Item(int id, string itemName, Boolean done)
        {
            this.id = id;
            this.itemName = itemName;
            this.done = done;
        }


        override public string ToString()
        {
            return string.Format("Item {0}, {1}, {2}", id, itemName, done);
        }

        public override bool Equals(object obj)
        {
            Item itemObj = obj as Item;
            if (itemObj == null)
            {
                return false;
            }
            else
            {
                return this.id == itemObj.id && this.itemName.Equals(itemObj.itemName) && this.done == itemObj.done;
            }
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

    }
}