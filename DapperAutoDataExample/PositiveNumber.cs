using System;
using System.Linq;
using AutoFixture;

namespace Api.Tests.Util
{
    public class PositiveNumber
    {
        public int Value;
        
        public PositiveNumber(int value)
        {
            this.Value = value;
        }

        public static implicit operator PositiveNumber(int y) => new PositiveNumber(y);
        public static implicit operator int(PositiveNumber x) => x.Value;
        public static implicit operator double(PositiveNumber x) => (double) x.Value;
        public static int Generate(IFixture fixture) => fixture.Create<Generator<int>>().Where(x => x > 0).Distinct().First();

    }

    public class FutureDateTimeOffset
    {
        public FutureDateTimeOffset(DateTimeOffset value)
        {
            Value = value;
        }

        public DateTimeOffset Value;
        public static DateTimeOffset Generate(IFixture fixture) =>
            fixture.Create<Generator<DateTimeOffset>>().Where(x => x > DateTimeOffset.Now).Distinct().First().AddDays(1);
        public static implicit operator DateTimeOffset(FutureDateTimeOffset x) => x.Value;
        public static implicit operator FutureDateTimeOffset(DateTimeOffset y) => new FutureDateTimeOffset(y);
    }
    public class PastDateTimeOffset
    {
        public PastDateTimeOffset(DateTimeOffset value)
        {
            Value = value;
        }

        public DateTimeOffset Value;
        public static DateTimeOffset Generate(IFixture fixture) =>
            fixture.Create<Generator<DateTimeOffset>>().Where(x => x < DateTimeOffset.Now).Distinct().First();
        public static implicit operator DateTimeOffset(PastDateTimeOffset x) => x.Value;
        public static implicit operator PastDateTimeOffset(DateTimeOffset y) => new PastDateTimeOffset(y);
    }
}