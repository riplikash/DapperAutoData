using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace DapperAutoData.ContentFiles;

/// <summary>
/// Attribute used to mark a test method for AutoFixture data generation.
/// </summary>
public class DapperAutoDataAttribute : InlineAutoDataAttribute
{
    public DapperAutoDataAttribute(params object[] values)
        : base(new AutoFixtureMoqDataAttribute(), values)
    {
    }

    public class AutoFixtureMoqDataAttribute : AutoDataAttribute
    {
        public AutoFixtureMoqDataAttribute()
            : base(() =>
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
            })
        {
        }
    }

}

[AttributeUsage(AttributeTargets.Method)]
public class AutoDomainDataAttribute : AutoDataAttribute
{
    public AutoDomainDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true }))
    {
    }
}