using AutoFixture;

namespace DapperAutoData.Generators;

public class PositiveIdentifier : IDataGenerator
{
    public int Value;

    public PositiveIdentifier(int value)
    {
        this.Value = value;
    }

    public PositiveIdentifier()
    {
    }

    public static implicit operator PositiveIdentifier(int y) => new PositiveIdentifier(y);
    public static implicit operator int(PositiveIdentifier x) => x.Value;
    public static implicit operator double(PositiveIdentifier x) => (double)x.Value;
    public static PositiveIdentifier Generate(IFixture fixture)
    {
        var number = fixture.Create<Generator<int>>();
        var selectedNumber = number.Where(x => x > 0).Distinct().Take(6);
        var total = 0;
        foreach (var i in selectedNumber)
        {
            total = 10 * total + i;
        }

        return total;
    }

    public override string? ToString()
    {
        return Value.ToString();
    }
    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => Generate(fixture));

}