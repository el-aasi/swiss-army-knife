using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using SCM.SwissArmyKnife.Extensions;

namespace ScadaMinds.SwissArmyKnife.Tests
{

    public class RandomExtensionsTests
    {
        private readonly Random _random = new Random();

        [Fact]
        public void NextDouble_ReturnsADouble_WithinSpecifiedRange()
        {
            // Run it a couple of times just to make sure it works always
            for (int i = 0; i < 100; i++)
            {
                var randomDouble = _random.NextDouble(0, 2);
                randomDouble.Should().BeGreaterThan(0);
                randomDouble.Should().BeLessThan(2);
            }
        }

        [Fact]
        public void NextByte_ReturnsAByte_WithinSpecifiedRange()
        {
            // Run it a couple of times just to make sure it works always
            for (int i = 0; i < 100; i++)
            {
                var randomDouble = _random.NextByte();
                randomDouble.Should().BeGreaterOrEqualTo(byte.MinValue);
                randomDouble.Should().BeLessOrEqualTo(byte.MaxValue);
            }
        }

        [Fact]
        public void NextBoolean_ReturnsABoolean()
        {
            // Run it a couple of times just to make sure it works always
            var randomlyGeneratedBooleans = Enumerable.Repeat(0, 100)
                .Select(i => _random.NextBoolean())
                .ToList();

            // Contains both trues and falses
            randomlyGeneratedBooleans.Should().Contain(true);
            randomlyGeneratedBooleans.Should().Contain(false);
        }

        [Fact]
        public void Choice_ReturnsARandomItem_FromEnumerable()
        {
            var list = new List<int>
            {
                1, 2, 3
            };

            for (int i = 0; i < 100; i++)
            {
                var randomChoice = _random.Choice(list);
                list.Should().Contain(randomChoice);
            }
        }
    }
}
