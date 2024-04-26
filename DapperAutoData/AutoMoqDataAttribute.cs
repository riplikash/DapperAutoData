using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace DapperAutoData;

/// <summary>
/// Attribute used to mark a test method for AutoFixture data generation.
/// </summary>
public class DapperAutoDataAttribute(params object[] values)
    : InlineAutoDataAttribute(new AutoFixtureMoqDataAttribute(), values)
{
    public class AutoFixtureMoqDataAttribute() : AutoDataAttribute(() =>
    {
        var fixture = new Fixture().Customize(new DefaultCustomizations());

        var customizationsTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IDapperProjectCustomization).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var customizationsType in customizationsTypes)
        {
            var customizations = (IDapperProjectCustomization)Activator.CreateInstance(customizationsType);
            fixture.Customize(customizations);
        }

        return fixture;
    });

}

[AttributeUsage(AttributeTargets.Method)]
public class AutoDomainDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true }));