using AutoFixture;

namespace DapperAutoData.Lib.Generators.Time;

public class PastDateTimeOffset : IDataGenerator
{
    public PastDateTimeOffset()
    {
    }

    public PastDateTimeOffset(DateTimeOffset value)
    {
        Value = value;
    }

    public DateTimeOffset Value;
    public static DateTimeOffset Generate(IFixture fixture) =>
        fixture.Create<Generator<DateTimeOffset>>().Where(x => x < DateTimeOffset.Now).Distinct().First();
    public static implicit operator DateTimeOffset(PastDateTimeOffset x) => x.Value;
    public static implicit operator PastDateTimeOffset(DateTimeOffset y) => new PastDateTimeOffset(y);
    public void RegisterGenerators(IFixture fixture) => fixture.Register<PastDateTimeOffset>(() => Generate(fixture));

}