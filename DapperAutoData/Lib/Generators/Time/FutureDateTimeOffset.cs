using AutoFixture;

namespace DapperAutoData.Lib.Generators.Time;

public class FutureDateTimeOffset : IDataGenerator
{
    public FutureDateTimeOffset()
    {
    }

    public FutureDateTimeOffset(DateTimeOffset value)
    {
        Value = value;
    }

    public DateTimeOffset Value;
    public static DateTimeOffset Generate(IFixture fixture) =>
        fixture.Create<Generator<DateTimeOffset>>().Where(x => x > DateTimeOffset.Now).Distinct().First().AddDays(1);
    public static implicit operator DateTimeOffset(FutureDateTimeOffset x) => x.Value;
    public static implicit operator FutureDateTimeOffset(DateTimeOffset y) => new FutureDateTimeOffset(y);
    public void RegisterGenerators(IFixture fixture) => fixture.Register<FutureDateTimeOffset>(() => Generate(fixture));

}