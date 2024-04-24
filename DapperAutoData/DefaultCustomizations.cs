using AutoFixture;
using AutoFixture.AutoMoq;

namespace DapperAutoData;

/// <summary>
/// Class that registers the default customizations for the AutoFixture instance.
/// Sets up AutoMoqCustomization and registers the data generators.
/// </summary>
public class DefaultCustomizations : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
        DataGeneratorInstaller.Run(fixture);
    }
}