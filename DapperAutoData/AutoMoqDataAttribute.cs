using AutoFixture;
using AutoFixture.Xunit2;

namespace DapperAutoData;

/// <summary>
/// Attribute used to mark a test method for AutoFixture data generation.
/// </summary>
public class DapperAutoDataAttribute(params object[] values)
    : InlineAutoDataAttribute(new AutoFixtureMoqDataAttribute(), values)
{
    public class AutoFixtureMoqDataAttribute() : AutoDataAttribute
    (FixtureFactory);

    private static IFixture FixtureFactory()
    {
        var fixture = new Fixture().Customize(new DefaultCustomizations());

        var customizationsTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(IDapperProjectCustomization).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var customizationsType in customizationsTypes)
        {
            var customizations = Activator.CreateInstance(customizationsType) as IDapperProjectCustomization;
            fixture.Customize(customizations);
        }

        return fixture;
    }
}
