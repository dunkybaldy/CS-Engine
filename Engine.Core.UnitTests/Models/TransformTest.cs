using Engine.Core.Models;
using FluentAssertions;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.UnitTests.Models
{
    [TestFixture]
    public class TransformTest
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TransformFunctions()
        {
            var Test2d = new Transform(Core.Models.Enums.EntityType._2D);
            var transformBy = new Vector2(2, -1);
            var expected = new Vector2(2, -1);

            Test2d.Position2d += transformBy;

            Test2d.Position2d.Should().Be(expected);
        }
    }
}
