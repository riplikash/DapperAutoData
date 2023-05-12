using AutoFixture;

namespace DapperAutoData.Lib.Generators;

public class PersonFirstName : IDataGenerator
{
    public PersonFirstName()
    {
        Value = "test";
    }

    public PersonFirstName(string value)
    {
        Value = value;
    }

    public string Value;
    public static string Generate(IFixture fixture) => $"{Faker.Name.First()}";
    public static implicit operator string(PersonFirstName x) => x.Value;
    public static implicit operator PersonFirstName(string y) => new(y);

    public override string ToString()
    {
        return Value;
    }

    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Register<PersonFirstName>(() => Generate(fixture));
    }
}