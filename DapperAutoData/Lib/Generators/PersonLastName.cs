using AutoFixture;

namespace DapperAutoData.Generators;

public class PersonLastName : IDataGenerator
{
    public PersonLastName()
    {
        Value = "test";
    }

    public PersonLastName(string value)
    {
        Value = value;
    }

    public string Value;
    public static string Generate(IFixture fixture) => $"{Faker.Name.Last()}";
    public static implicit operator string(PersonLastName x) => x.Value;
    public static implicit operator PersonLastName(string y) => new(y);

    public override string ToString()
    {
        return Value;
    }

    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Register<PersonLastName>(() => Generate(fixture));
    }
}