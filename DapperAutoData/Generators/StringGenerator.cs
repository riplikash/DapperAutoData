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

public class StringParagraph : StringGenerator<StringParagraph> { public StringParagraph() : base(faker => faker.Lorem.Paragraph()) { } }
public class StringPersonFullName : StringGenerator<StringPersonFullName> { public StringPersonFullName() : base(faker => faker.Name.FullName()) { } }
public class StringPhoneNumber : StringGenerator<StringPhoneNumber> { public StringPhoneNumber() : base(faker => faker.Phone.PhoneNumber()) { } }
public class StringSentence : StringGenerator<StringSentence> { public StringSentence() : base(faker => faker.Lorem.Sentence()) { } }
public class StringSsn : StringGenerator<StringSsn> { public StringSsn() : base(faker => $"{faker.Random.Number(100, 999)}-{faker.Random.Number(10, 99)}-{faker.Random.Number(1000, 9999)}") { } }
public class StringWord : StringGenerator<StringWord> { public StringWord() : base(faker => faker.Lorem.Word()) { } }
public class StringCompanyName : StringGenerator<StringCompanyName> { public StringCompanyName() : base(faker => faker.Company.CompanyName()) { } }
public class StringEmailTest : StringGenerator<StringEmailTest> { public StringEmailTest() : base(faker => $"{faker.Internet.DomainName()}@FakeEmailAddress.com") { } }
public class StringFirstName : StringGenerator<StringFirstName> { public StringFirstName() : base(faker => faker.Name.FirstName()) { } }
public class StringInternetUrl : StringGenerator<StringInternetUrl> { public StringInternetUrl() : base(faker => faker.Internet.Url()) { } }
public class StringInternetUsername : StringGenerator<StringInternetUsername> { public StringInternetUsername() : base(faker => faker.Internet.UserName()) { } }

public class StringCityName : StringGenerator<StringCityName> { public StringCityName() : base(faker => faker.Address.City()) { } }
public class StringCountryName : StringGenerator<StringCountryName> { public StringCountryName() : base(faker => faker.Address.Country()) { } }
public class StringPostalCode : StringGenerator<StringPostalCode> { public StringPostalCode() : base(faker => faker.Address.ZipCode()) { } }
public class StringStateAbbreviation : StringGenerator<StringStateAbbreviation> { public StringStateAbbreviation() : base(faker => faker.Address.StateAbbr()) { } }
public class StringStateFullName : StringGenerator<StringStateFullName> { public StringStateFullName() : base(faker => faker.Address.State()) { } }
public class StringStreetAddress : StringGenerator<StringStreetAddress> { public StringStreetAddress() : base(faker => faker.Address.StreetAddress()) { } }
public class StringJobTitle : StringGenerator<StringJobTitle> { public StringJobTitle() : base(faker => faker.Name.JobTitle()) { } }
public class StringProductCategory : StringGenerator<StringProductCategory> { public StringProductCategory() : base(faker => faker.Commerce.Categories(1)[0]) { } }
public class StringProductDescription : StringGenerator<StringProductDescription> { public StringProductDescription() : base(faker => faker.Commerce.ProductDescription()) { } }
public class StringCurrency : StringGenerator<StringCurrency> { public StringCurrency() : base(faker => faker.Finance.Currency().Code) { } } // Extracting the currency code as a string
public class StringFileExtension : StringGenerator<StringFileExtension> { public StringFileExtension() : base(faker => faker.System.CommonFileExt()) { } }
public class StringIPAddress : StringGenerator<StringIPAddress> { public StringIPAddress() : base(faker => faker.Internet.Ip()) { } }
public class StringHtmlTag : StringGenerator<StringHtmlTag> { public StringHtmlTag() : base(faker => faker.Random.AlphaNumeric(5)) { } }
public class StringPassword : StringGenerator<StringPassword> { public StringPassword() : base(faker => faker.Internet.Password()) { } }
public class StringGuid : StringGenerator<StringGuid> { public StringGuid() : base(faker => faker.Random.Guid().ToString()) { } }