using AutoFixture;
using Bogus;

namespace DapperAutoData.Generators;

/// <summary>
/// Set of classes that generate numbers.
/// </summary>
/// <typeparam name="T"></typeparam>
public class NumberGenerator<T> : IDataGenerator
{
    public decimal Value { get; }
    private readonly Func<Faker, decimal> _generator;

    public NumberGenerator(decimal value) => (Value, _generator) = (value, _ => value);
    public NumberGenerator(Func<Faker, decimal> generator) => (Value, _generator) = (generator(new Faker()), generator);

    public static implicit operator NumberGenerator<T>(decimal y) => new(y);
    public static implicit operator decimal(NumberGenerator<T> x) => x.Value;
    public static implicit operator int(NumberGenerator<T> x) => (int)x.Value;
    public static implicit operator float(NumberGenerator<T> x) => (float)x.Value;

    public static NumberGenerator<T> Generate(IFixture fixture, Func<Faker, decimal> generator) => new(generator);

    public override string? ToString() => Value.ToString();
    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => Generate(fixture, _generator));
}

public class NumberPositive : NumberGenerator<NumberPositive> { public NumberPositive() : base(faker => faker.Random.Decimal(1, decimal.MaxValue)) { } }
public class NumberNegative : NumberGenerator<NumberNegative> { public NumberNegative() : base(faker => faker.Random.Decimal(decimal.MinValue, -1)) { } }
public class NumberMoney : NumberGenerator<NumberMoney> { public NumberMoney() : base(GenerateFakeMoney) { } private static decimal GenerateFakeMoney(Faker faker) => faker.Random.Decimal(1, 1000) + faker.Random.Decimal(0.01m, 0.99m); }
public class NumberFraction : NumberGenerator<NumberFraction> { public NumberFraction() : base(faker => Math.Round(faker.Random.Decimal(0.01m, 0.99m), 2)) { } }