using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Common.Tests.Extensions.LookupValueExtensionsTests
{
    [TestClass]
    public class LookupValueExtensionsTests
    {
        [TestMethod]
        public void ToSqlCSVReturnsValue()
        {
            var sut = new List<LookupValue<int>>()
            {
                new LookupValue<int>{Id = 1},
                new LookupValue<int>{Id = 4}
            };

            sut.ToSqlCsv().Should().Be("1,4");
        }

        [TestMethod]
        public void ToSqlCSVReturnsEmptyStringWhenEmptyList()
        {
            var sut = new List<LookupValue<int>>()
            {
                
            };

            sut.ToSqlCsv().Should().BeEmpty();
        }

        [TestMethod]
        public void ToSqlCSVReturnsEmptyStringWhenNull()
        {
            List<LookupValue<int>> sut = null;

            sut.ToSqlCsv().Should().BeEmpty();
        }



        [TestMethod]
        public void ToCsvReturnsIds()
        {
            var sut = new List<LookupValue<int>>()
            {
                new LookupValue<int>{Id = 1, Name = "Item 1"},
                new LookupValue<int>{Id = 4, Name = "Item 4"}
            };

            sut.ToCsv(x => x.Id).Should().Be("1,4");
        }

        [TestMethod]
        public void ToCsvReturnsName()
        {
            var sut = new List<LookupValue<int>>()
            {
                new LookupValue<int>{Id = 1, Name = "Item 1"},
                new LookupValue<int>{Id = 4, Name = "Item 4"}
            };

            sut.ToCsv(x => x.Name).Should().Be("Item 1,Item 4");
        }

        [TestMethod]
        public void ToCsvReturnsEmptyStringWhenNull()
        {
            List<LookupValue<int>> sut = null;

            sut.ToCsv(x => x.Id).Should().BeEmpty();
        }

        [TestMethod]
        public void ToCsvWithCustomDelimiter()
        {
            var sut = new List<LookupValue<int>>()
            {
                new LookupValue<int>{Id = 1, Name = "Item 1"},
                new LookupValue<int>{Id = 4, Name = "Item 4"}
            };

            sut.ToCsv(x => x.Name, ", ").Should().Be("Item 1, Item 4");
        }
    }
}
