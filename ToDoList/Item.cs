using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ToDoList
{
    public class Item
    {
        [IgnoreDataMember]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int itemId { get; set; }
        public string itemName { get; set; }
        public Boolean done { get; set; }

        public Item(int id, string itemName, Boolean done)
        {
            this.itemId = id;
            this.itemName = itemName;
            this.done = done;
        }


        override public string ToString()
        {
            return string.Format("Item {0}, {1}, {2}", itemId, itemName, done);
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
                return this.itemId == itemObj.itemId && this.itemName.Equals(itemObj.itemName) && this.done == itemObj.done;
            }
        }

        public override int GetHashCode()
        {
            return this.itemId.GetHashCode();
        }

    }
}