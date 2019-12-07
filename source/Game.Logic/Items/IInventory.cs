using System.Collections.Generic;

namespace Game.Logic.Items
{
    public interface IInventory
    {
        int AddItem(Item item);
        void RemoveItem(string id, int count = 1);
        Item GetItem(string id);
        bool ContainsItem(string id);
        IEnumerable<Item> ListAllItems();
        IEnumerable<Item> ListCanUseItems();
    }
}