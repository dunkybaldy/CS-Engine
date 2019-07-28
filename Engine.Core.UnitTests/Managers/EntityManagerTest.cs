using Engine.Core.Managers.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core.Tests.Managers
{
    [TestFixture]
    public class EntityManagerTest : SetUpFixture
    {
        private IEntityManager _entityManager;

        [SetUp]
        public void Setup()
        {
            _entityManager = GetService<IEntityManager>();
        }

        [Test]
        public void RetrievingUninitialisedEntityThrowsError()
        {
            var exceptionThrown = false;

            try
            {
                _entityManager.GetEntity("I Don't Exist");
            }
            catch (Exception ex)
            {
                ex.GetType().Name.ToString().Should().Be("KeyNotFoundException");
                exceptionThrown = true;
            }

            exceptionThrown.Should().BeTrue();
        }

        [Test]
        public void UpdateAllEntities()
        {

        }
    }
}
