using AutoFixture;

namespace DapperAutoData.Lib.Generators;

public class PositiveNumber : IDataGenerator
{
    public int Value;

    public PositiveNumber(int value)
    {
        Value = value;
    }

    public PositiveNumber()
    {
    }

    public static implicit operator PositiveNumber(int y) => new PositiveNumber(y);
    public static implicit operator int(PositiveNumber x) => x.Value;
    public static implicit operator double(PositiveNumber x) => x.Value;
    public static PositiveNumber Generate(IFixture fixture) => fixture.Create<Generator<int>>().Where(x => x > 0).Distinct().First();
    public override string? ToString()
    {
        return Value.ToString();
    }
    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => Generate(fixture));

}