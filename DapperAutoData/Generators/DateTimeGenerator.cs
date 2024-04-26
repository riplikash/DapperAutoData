using AutoFixture;
using Bogus;

namespace DapperAutoData.Generators;

/// <summary>
///     Set of classes that generate DateTime and DateTimeOffset values.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DateTimeGenerator<T>(DateTime value) : IDataGenerator
{
    public DateTime Value { get; } = value;

    private readonly Func<Faker, DateTime> _generator = null!;

    public DateTimeGenerator(Func<Faker, DateTime> generator) : this(generator(new Faker()))
    {
        _generator = generator;
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

public class DateTimeOffsetGenerator<T>(DateTimeOffset value) : IDataGenerator
{
    public DateTimeOffset Value { get; } = value;

    private readonly Func<Faker, DateTimeOffset> _generator;

    public DateTimeOffsetGenerator(Func<Faker, DateTimeOffset> generator) : this(generator(new Faker()))
    {
        _generator = generator;
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

public class DateTimeFuture() : DateTimeGenerator<DateTimeFuture>(faker => faker.Date.Future());

public class DateTimeOffsetFuture() : DateTimeOffsetGenerator<DateTimeOffsetFuture>(faker => faker.Date.FutureOffset());

public class DateTimeOfBirth() : DateTimeGenerator<DateTimeOfBirth>(faker =>
    faker.Date.Between(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-18)));

public class DateTimeOffsetOfBirth() : DateTimeOffsetGenerator<DateTimeOffsetOfBirth>(faker =>
    faker.Date.BetweenOffset(DateTime.Now.AddYears(-80), DateTime.Now.AddYears(-18)));

public class DateTimePast() : DateTimeGenerator<DateTimePast>(faker => faker.Date.Past());

public class DateTimeOffsetPast() : DateTimeOffsetGenerator<DateTimeOffsetPast>(faker => faker.Date.PastOffset());

public class CurrentDate() : DateTimeGenerator<CurrentDate>(DateTime.Now);

public class DateSpecialEvent() : DateTimeGenerator<DateSpecialEvent>(new DateTime(2023, 12, 25))
{
    // Example: Christmas
}

public class DateWeekday() : DateTimeGenerator<DateWeekday>(GenerateWeekdayDate)
{
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

public class DateWeekend() : DateTimeGenerator<DateWeekend>(GenerateWeekendDate)
{
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

public class DateLeapYear() : DateTimeGenerator<DateLeapYear>(new DateTime(2024, 2, 29))
{
    // Example: Leap year date
}

public class DateCustomFormat()
    : DateTimeGenerator<DateCustomFormat>(faker => DateTime.Parse(faker.Date.Recent(7).ToShortDateString()));