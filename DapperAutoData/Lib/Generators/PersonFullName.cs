using AutoFixture;

namespace DapperAutoData.Generators;

public class PersonFullName : IDataGenerator
{
    public PersonFullName()
    {
        Value = "test";
    }

    public PersonFullName(string value)
    {
        Value = value;
    }

    public string Value;
    public static string Generate(IFixture fixture) => $"{Faker.Name.FullName()}";
    public static implicit operator string(PersonFullName x) => x.Value;
    public static implicit operator PersonFullName(string y) => new(y);

    public override string ToString()
    {
        return Value;
    }

    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Register<PersonFullName>(() => Generate(fixture));
    }
}