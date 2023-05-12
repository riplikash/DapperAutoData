using AutoFixture;
using Faker;

namespace DapperAutoData.Generators;

public class TestEmail : IDataGenerator
{
    public TestEmail()
    {
        Value = "test";
    }

    public TestEmail(string value)
    {
        Value = value;
    }

    public string Value;
    public static string Generate(IFixture fixture) => $"{Internet.DomainName()}@DAPPERTEST.com";
    public static implicit operator string(TestEmail x) => x.Value;
    public static implicit operator TestEmail(string y) => new(y);

    public override string ToString()
    {
        return Value;
    }

    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Register<TestEmail>(() => Generate(fixture));
    }
}