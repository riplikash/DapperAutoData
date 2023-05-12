using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace DapperAutoData.Content;

public class DapperAutoDataAttribute : InlineAutoDataAttribute
{
    public DapperAutoDataAttribute(params object[] values)
        : base(new AutoFixtureMoqDataAttribute(), values)
    {
    }

    public class AutoFixtureMoqDataAttribute : AutoDataAttribute
    {
        public AutoFixtureMoqDataAttribute()
            : base(() => new Fixture()
                .Customize(new DefaultCustomizations())
                .Customize(new ProjectCustomizations())
            )
        {
        }
    }

    internal class ProjectCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.RepeatCount = 3;
            fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            DataGeneratorInstaller.Run(fixture);
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