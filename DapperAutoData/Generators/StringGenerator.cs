using AutoFixture;
using Bogus;

namespace DapperAutoData.Generators;

/// <summary>
/// Set of data generators for various string types. These generators are based on the Bogus library.
/// </summary>
/// <typeparam name="T"></typeparam>
public class StringGenerator<T> : IDataGenerator
{
    private readonly Func<Faker, string> _generator;
    public string Value { get; }

    public StringGenerator(string value = "") => (Value, _generator) = (value, faker => value);
    public StringGenerator(Func<Faker, string> generator) => (Value, _generator) = (generator(new Faker()), generator);

    public static implicit operator string(StringGenerator<T> x) => x.Value;
    public static implicit operator StringGenerator<T>(string y) => new(y);

    public void RegisterGenerators(IFixture fixture) => fixture.Register(() => new StringGenerator<T>(_generator));
}

public class StringParagraph() : StringGenerator<StringParagraph>(faker => faker.Lorem.Paragraph());
public class StringPersonFullName() : StringGenerator<StringPersonFullName>(faker => faker.Name.FullName());
public class StringPhoneNumber() : StringGenerator<StringPhoneNumber>(faker => faker.Phone.PhoneNumber());
public class StringSentence() : StringGenerator<StringSentence>(faker => faker.Lorem.Sentence());
public class StringSsn() : StringGenerator<StringSsn>(faker => $"{faker.Random.Number(100, 999)}-{faker.Random.Number(10, 99)}-{faker.Random.Number(1000, 9999)}");
public class StringWord() : StringGenerator<StringWord>(faker => faker.Lorem.Word());
public class StringCompanyName() : StringGenerator<StringCompanyName>(faker => faker.Company.CompanyName());
public class StringEmailTest() : StringGenerator<StringEmailTest>(faker => $"{faker.Internet.DomainName()}@FakeEmailAddress.com");
public class StringFirstName() : StringGenerator<StringFirstName>(faker => faker.Name.FirstName());
public class StringInternetUrl() : StringGenerator<StringInternetUrl>(faker => faker.Internet.Url());
public class StringInternetUsername() : StringGenerator<StringInternetUsername>(faker => faker.Internet.UserName());

public class StringCityName() : StringGenerator<StringCityName>(faker => faker.Address.City());
public class StringCountryName() : StringGenerator<StringCountryName>(faker => faker.Address.Country());
public class StringPostalCode() : StringGenerator<StringPostalCode>(faker => faker.Address.ZipCode());
public class StringStateAbbreviation() : StringGenerator<StringStateAbbreviation>(faker => faker.Address.StateAbbr());
public class StringStateFullName() : StringGenerator<StringStateFullName>(faker => faker.Address.State());
public class StringStreetAddress() : StringGenerator<StringStreetAddress>(faker => faker.Address.StreetAddress());
public class StringJobTitle() : StringGenerator<StringJobTitle>(faker => faker.Name.JobTitle());
public class StringProductCategory() : StringGenerator<StringProductCategory>(faker => faker.Commerce.Categories(1)[0]);
public class StringProductDescription() : StringGenerator<StringProductDescription>(faker => faker.Commerce.ProductDescription());
public class StringCurrency() : StringGenerator<StringCurrency>(faker => faker.Finance.Currency().Code); // Extracting the currency code as a string
public class StringFileExtension() : StringGenerator<StringFileExtension>(faker => faker.System.CommonFileExt());
public class StringIPAddress() : StringGenerator<StringIPAddress>(faker => faker.Internet.Ip());
public class StringHtmlTag() : StringGenerator<StringHtmlTag>(faker => faker.Random.AlphaNumeric(5));
public class StringPassword() : StringGenerator<StringPassword>(faker => faker.Internet.Password());
public class StringGuid() : StringGenerator<StringGuid>(faker => faker.Random.Guid().ToString());