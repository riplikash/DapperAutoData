using System;
using Api.Tests.Util;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Api.Tests
{
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
                        .Customize(new ProjectCustomizations())
                    //.Customize(new ConstructorCustomization(typeof (ExampleController), new GreedyConstructorQuery()))                
                )
            {
            }
        }
        internal class ProjectCustomizations : ICustomization
        {
            public void Customize(IFixture fixture)
            {

                fixture.RepeatCount = 5;
                fixture.Customize(new MultipleCustomization())
                    .Customize(new AutoConfiguredMoqCustomization() { ConfigureMembers = true });
                fixture.Register<PositiveNumber>(() => PositiveNumber.Generate(fixture));
                fixture.Register<FutureDateTimeOffset>(() => FutureDateTimeOffset.Generate(fixture));
                fixture.Register<PastDateTimeOffset>(() => PastDateTimeOffset.Generate(fixture));
             
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
}
