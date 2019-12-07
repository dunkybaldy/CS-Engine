using FluentAssertions;
using Game.Logic.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.UnitTests
{
    [TestFixture]
    public class InventoryTests : SetUpFixture
    {
        private IInventory Inventory;

        [SetUp]
        public void Setup()
        {
            Inventory = GetService<IInventory>();
        }

        [Test]
        public void CannotAddMoreThanMaxStacks()
        {
            var maxStack = 10;
            var currentCount = 6;
            var itemId = "Item1";
            var item = new Item(maxStack, currentCount)
            {
                Id = itemId,
            };

            Inventory.AddItem(item);
            var a = Inventory.GetItem(item.Id);
            a.CurrentCount.Should().Be(6);
            a.IsAtMaxStack.Should().BeFalse();

            var anyLeft = Inventory.AddItem(item);

            a.CurrentCount.Should().Be(item.MaxStack);
            a.IsAtMaxStack.Should().BeTrue();
            Inventory.ListAllItems().Should().HaveCount(1);
            anyLeft.Should().Be((currentCount * 2) - maxStack);
        }

        [Test]
        public void RemovingAnItemWhichIsNotThereDoesntThrowException()
        {
            var exThrown = false;
            try
            {
                Inventory.RemoveItem("Item1");
            }
            catch (Exception)
            {
                exThrown = true;
            }

            exThrown.Should().BeFalse();
        }
    }
}
