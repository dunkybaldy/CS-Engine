using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Items
{
    public class Item
    {
        public string Id { get; set; }
        public bool Disabled { get; set; }
        public bool CanBeDropped { get; protected set; }
        public int CurrentCount { get; protected set; }
        public int MaxStack { get; protected set; }
        public bool Stackable => MaxStack == 1;
        public bool IsAtMaxStack => CurrentCount >= MaxStack;

        public Item(int maxStack = 99, int currentCount = 0, bool canBeDropped = true)
        {
            MaxStack = maxStack;
            CurrentCount = currentCount;
            CanBeDropped = canBeDropped;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">How much the item should increase by</param>
        /// <returns>Number of left over items</returns>
        public int IncreaseCount(int num)
        {
            var itemsNotUsed = 0;
            if (CurrentCount + num > MaxStack)
            {
                itemsNotUsed = CurrentCount + num - MaxStack;
                CurrentCount = MaxStack;
            }
            else
                CurrentCount += num;
            return itemsNotUsed;
        }

        public int DecreaseCount(int num)
        {
            var itemOverhead = 0;
            if (CurrentCount - num < 0)
            {
                itemOverhead = num - CurrentCount;
                CurrentCount = 0;
            }
            else
                CurrentCount -= num;
            return itemOverhead;
        }

        public virtual void Use() {}
    }
}
