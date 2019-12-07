using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.Items
{
    public class Inventory : IInventory
    {
        public Dictionary<string, Item> Items { get; private set; }
        public int MaxNumberOfItems { get; private set; }

        public Inventory(int maxNumberOfItems)
        {
            Items = new Dictionary<string, Item>();
            MaxNumberOfItems = maxNumberOfItems;
        }

        public int AddItem(Item item)
        {
            if (ContainsItem(item.Id))
                return IncreaseItemCount(item.Id, item.CurrentCount);
            else
                Items.Add(item.Id, item);

            return 0;
        }

        public void RemoveItem(string id, int count = 1)
        {
            if (ContainsItem(id))
                DecreaseItemCount(id, count);
        }

        public Item GetItem(string id)
        {
            if (Items.TryGetValue(id, out Item value))
                return value;
            return null;
        }

        public bool ContainsItem(string id)
        {
            if (Items.ContainsKey(id))
                return true;
            return false;
        }

        public IEnumerable<Item> ListAllItems() => Items.Values.ToList();
        public IEnumerable<Item> ListCanUseItems() => Items.Values.Where(x => !x.Disabled);

        private int IncreaseItemCount(string id, int count)
        {
            if (Items.TryGetValue(id, out Item value))
                return value.IncreaseCount(count);
            return count;
        }

        private void DecreaseItemCount(string id, int count)
        {
            if (Items.TryGetValue(id, out Item value))
                value.DecreaseCount(count);
        }
    }
}
