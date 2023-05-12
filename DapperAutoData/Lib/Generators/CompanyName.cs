using AutoFixture;

namespace DapperAutoData.Generators;

public class CompanyName : IDataGenerator
{
    public static string Generate() => Faker.Company.Name();

    public CompanyName(string value)
    {
        Value = value;
    }

    public CompanyName()
    {
        Value = "";
    }
    public string Value;
    public static implicit operator string(CompanyName x) => x.Value;
    public static implicit operator CompanyName(string y) => new CompanyName(y);
    public void RegisterGenerators(IFixture fixture) => fixture.Register<CompanyName>(() => Generate());

}