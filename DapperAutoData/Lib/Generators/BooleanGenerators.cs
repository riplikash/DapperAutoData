using AutoFixture;

namespace DapperAutoData.Lib.Generators;

public class BooleanGenerators : IDataGenerator
{
    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Register(() => fixture.Create<int>() % 2 == 0);
    }
}