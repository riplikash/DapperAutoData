using AutoFixture;

namespace DapperAutoData;

/// <summary>
/// Marker interface for data generators. Any class implementing this interface will be registered as a data generator.
/// </summary>
public interface IDataGenerator
{
    public void RegisterGenerators(IFixture fixture);
}