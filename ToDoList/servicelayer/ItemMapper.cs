using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList
{
    class ItemMapper
    {

        public static List<Item> MapToListOfItems(List<string[]> inputTableData)
        {
            if (inputTableData.Count == 0)
            {
                return new List<Item>();
            }

            return inputTableData.Select(tableRow => {
                return MapToItem(tableRow);
            }).ToList();
        }

        public static Item MapToItem(string[] inputTableRow)
        {
            if (inputTableRow == null)
            {
                return null;
            }

            return new Item(Int32.Parse(inputTableRow[0]), inputTableRow[1], Int32.Parse(inputTableRow[2]) == 1 ? true : false);
        }
    }
}