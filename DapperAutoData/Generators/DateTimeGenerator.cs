using AutoFixture;
using Bogus;

namespace DapperAutoData.Generators;

/// <summary>
///     Set of classes that generate DateTime and DateTimeOffset values.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DateTimeGenerator<T> : IDataGenerator
{
    public DateTime Value { get; }

    private readonly Func<Faker, DateTime> _generator = null!;

    public DateTimeGenerator(DateTime value)
    {
        Value = value;
    }

    public DateTimeGenerator(Func<Faker, DateTime> generator)
    {
        _generator = generator;
        Value = generator(new Faker());
    }

    public static implicit operator DateTimeGenerator<T>(DateTime y) => new(y);
    public static implicit operator DateTime(DateTimeGenerator<T> x) => x.Value;

    public static DateTimeGenerator<T> Generate(IFixture fixture, Func<Faker, DateTime> generator) => new(generator);

    public override string? ToString()
    {
        return Value.ToString();
    }

    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => Generate(fixture, _generator));
}

public class DateTimeOffsetGenerator<T> : IDataGenerator
{
    public DateTimeOffset Value { get; }

    private readonly Func<Faker, DateTimeOffset> _generator;

    public DateTimeOffsetGenerator(DateTimeOffset value)
    {
        Value = value;
    }

    public DateTimeOffsetGenerator(Func<Faker, DateTimeOffset> generator)
    {
        _generator = generator;
        Value = generator(new Faker());
    }

    public static implicit operator DateTimeOffsetGenerator<T>(DateTimeOffset y) => new(y);
    public static implicit operator DateTimeOffset(DateTimeOffsetGenerator<T> x) => x.Value;

    public static DateTimeOffsetGenerator<T> Generate(IFixture fixture, Func<Faker, DateTimeOffset> generator) => new(generator);

    public override string? ToString()
    {
        return Value.ToString();
    }

    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => Generate(fixture, _generator));
}

public class DateTimeFuture : DateTimeGenerator<DateTimeFuture>
{
    public DateTimeFuture() : base(faker => faker.Date.Future()) { }
}

public class DateTimeOffsetFuture : DateTimeOffsetGenerator<DateTimeOffsetFuture>
{
    public DateTimeOffsetFuture() : base(faker => faker.Date.FutureOffset()) { }
}

public class DateTimeOfBirth : DateTimeGenerator<DateTimeOfBirth>
{
    public DateTimeOfBirth() : base(faker => faker.Date.Between(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-18))) { }
}

public class DateTimeOffsetOfBirth : DateTimeOffsetGenerator<DateTimeOffsetOfBirth>
{
    public DateTimeOffsetOfBirth() : base(faker => faker.Date.BetweenOffset(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-18))) { }
}

public class DateTimePast : DateTimeGenerator<DateTimePast>
{
    public DateTimePast() : base(faker => faker.Date.Past()) { }
}

public class DateTimeOffsetPast : DateTimeOffsetGenerator<DateTimeOffsetPast>
{
    public DateTimeOffsetPast() : base(faker => faker.Date.PastOffset()) { }
}

public class CurrentDate : DateTimeGenerator<CurrentDate>
{
    public CurrentDate() : base(DateTime.Now) { }
}

public class DateSpecialEvent : DateTimeGenerator<DateSpecialEvent>
{
    public DateSpecialEvent() : base(new DateTime(2023, 12, 25)) { } // Example: Christmas
}

public class DateWeekday : DateTimeGenerator<DateWeekday>
{
    public DateWeekday() : base(GenerateWeekdayDate) { }

    private static DateTime GenerateWeekdayDate(Faker faker)
    {
        DateTime date;
        do
        {
            date = faker.Date.Future();
        } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        return date;
    }
}

public class DateWeekend : DateTimeGenerator<DateWeekend>
{
    public DateWeekend() : base(GenerateWeekendDate) { }

    private static DateTime GenerateWeekendDate(Faker faker)
    {
        DateTime date;
        do
        {
            date = faker.Date.Future();
        } while (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);
        return date;
    }
}

public class DateLeapYear : DateTimeGenerator<DateLeapYear>
{
    public DateLeapYear() : base(new DateTime(2024, 2, 29)) { } // Example: Leap year date
}

public class DateCustomFormat : DateTimeGenerator<DateCustomFormat>
{
    public DateCustomFormat() : base(faker => DateTime.Parse(faker.Date.Recent(7).ToShortDateString())) { }
}