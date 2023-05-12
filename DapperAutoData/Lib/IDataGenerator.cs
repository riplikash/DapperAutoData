using AutoFixture;

namespace DapperAutoData;

public interface IDataGenerator
{
    public void RegisterGenerators(IFixture fixture);
}