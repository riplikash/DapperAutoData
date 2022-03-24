using System;
using Api.Tests;
using FluentAssertions;
using Xunit;

namespace RollAppUnitTests
{
    public class UnitTest1
    {
        [Theory]
        [DapperAutoData(1)]
        public void Test1(int a, int b)
        {
            var c = a + b;
            c.Should().Be(1 + b);
        }
    }
}
