# Dapper Testing Library

As a fan of TDD, BDD, and anonymous testing I've made heavy use of AutoFixture, AutoMoq, and Fluent Assertions in my projects since around 2015. I've been lucky enough to work with a few other developers across these companies, and we found we kept needing to implement the same patterns and utilites every time we moved to a new project. We even lost some work and knowledge along the way. This library is an attempt to consolidate all of that knowledge into a single library that can be used across all of our projects. It's a work in progress, but I hope it can be useful to others as well.

DapperAutoData (yes, unrelated to Dapper ORM. I was using the name first!) is a library that simplifies the process of setting up tests and generating data for your tests. It is built on top of AutoFixture, AutoMoq, and Fluent Assertions, and provides a set of attributes that you can use to automatically generate data for your tests. 

It sets up patterns for creating data generators, customizing data generation, and testing for exceptions. It also provides a set of built-in data generators that you can use to generate specific types of data for your tests. You can also create your own data generators to generate custom data for your tests.

To be clear, I in no way intend for this to be seen as novel work or take credit for anything done by Mark Seemann, or any of the other developers who have worked on these projects. This is simply a consolidation of the work they have done, and a way to make it easier to use across all of our projects, with the hope that it might provide some value to others. If any of the authors of the libraries used here have ANY concerns with this use, please reach out to me and I'll do my best to rectify it, or even take this repo down.

## Features

-   **AutoFixture** to generate anonymous data for your tests.
-   **AutoMoq** for automocking in your unit tests.
-   **Fluent Assertions** for more readable assertions.
-   **Moq** for manual mocking.
-   **Custom Data Generators** for generating specific types of data.
-   **Bosus** for generating fake data.

## Useful Links

-   [AutoFixture Documentation](https://github.com/AutoFixture/AutoFixture/wiki)
-   [Fluent Assertions Documentation](https://fluentassertions.com/documentation)
-   [Faker.Net Documentation](https://github.com/bchavez/Bogus)
-   [AutoMoq Documentation](https://github.com/Moq/moq4/wiki/Quickstart)

## Installation

In package manager console run the command: `Install-Package DapperAutoData` 

The library is distributed as a nuget package. After installing the package all you need to do is add the `DapperAutoData` attribute to your `Theory` based test methods. Behavior can be modified by adding implementations of `IDataGenerator` and `IDapperProjectCustomization` to your test project. They will be automatically picked up by the library.

The DataGeneratorInstaller will scan for, and use any implementation of IDataGenerator in the current assembly.

## How to Use

### Behavior Customization

Autofixture can normally be customized by calling passing in an ICustimization to the fixture. The DapperAutoData will attribute will automatically apply any customizations that implement the IDapperProjectCustomization interface, which is a wrapper for ICustomization.
```
public class DapperDataCustomizations : IDapperProjectCustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.RepeatCount = 3;
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}
```

### Creating a Data Generator

A major reason for using the Dapper Testing Library is to create custom data generators. As a project grows, it will build up a library of generators that can be used to create complex objects for testing. Each generator can be self-contained and updated individually, which allows for a team to focus only on the test cases they are working on, knowing that any data the test needs will be provided by the library in a valid state. Developers should take on the responsibility for creating and updating generators for the objects they are working with. They will be automatically picked up for use in the tests.

Generally, we follow one of two patterns for creating data generators. Initially the pattern was to to create a generator that explicitly defines EVERYTHING about an object. In practice, we've found this to be a bit cumbersome.

So we have moved to a second pattern, where we create a generator that defines the minimum required fields that need to be customized. This makes it easier to refactor classes when there aren't a ton of customized fields.

Here is an example of a generator that defines the minimum required fields, and then allows the developer to customize the rest of the object as needed. This is the preferred pattern where possible:

```
public class ExampleInfoGenerator : IDataGenerator
{
    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Customize<ExampleInfo>(c => c
            .With(x => x.UserId, fixture.Create<NumberPositive>().Value.ToString)
            .With(x => x.Name, fixture.Create<StringCompanyName>())
        );
    }
}
```

And here is an example of a generator that defines everything about an object. This can be a bit cumbersome for larger data objects, like POCOs and DTOs. Note that this is still a valuable pattern when defining more complex objects. For example, if a related object needs to be generated first and the data linked, or when there are dependencies between properties. 

```
// Notice that in this example we have to define Date and AccountBalance, even though we are not meaningfully customizing them.
// However, for this particular object the UserId must always be prefixed with the AccountType.Code.
// So despite the extra overhead, in this example it is still the best pattern to use, as it ensures that the UserId is always correct.
public class ExampleInfoGenerator
{
    public ExampleInfo GenerateExampleInfo()
    {
        var accountType = fixture.Create<AccountType>();
        fixture.Register(() => new ExampleInfo
		{
			UserId = $"{accountType.Code}{fixture.Create<NumberPositive>().Value.ToString()},
			Name = fixture.Create<StringCompanyName>(),
            Date = fixture.Create<DateTime>(),
            AccountBalance = fixture.Create<double>(),
            AccountType = accountType
		});
    }
}
```

### Using Dapper Testing Library in your Tests

The Dapper Testing Library is designed to be easy to use. It is effectively a wrapper around AutoFixture, AutoMoq, and Fluent Assertions that simplifies the process of setting up tests and generating data. You can start using it in your tests by adding the `DapperAutoData` attribute to your test methods. This will automatically generate data for your tests using AutoFixture and AutoMoq.

Keep in mind that the `DapperAutoData` attribute is designed to work with `Theory` based tests. You can learn more about how to set up  

You can read more about how to use AutoFixture [here](https://autofixture.github.io/docs/quick-start/).

Here are some examples of tests using the Dapper Testing Library:

#### Example of a Unit Test

```
/// This test will create an account with the minimum required fields and assert that no validation errors are returned.
[Theory]
[DapperAutoData]
public void Create_MinimumRequiredFields_NoValidationErrors(CreateAccountRequest request)
{
    // Arrange

    // Act
    var account = new AccountEntity(request);

    // Assert
    account.Validate().Items.Should().BeNullOrEmpty();
    account.LoginEmail.Should().Be(request.LoginEmail);
    account.Address.Should().Be(request.Address);
    account.ContactInfo.Should().Be(request.ContactInfo);
}
``` 

#### Example of a Theory Driven Test

```
/// This test will create an account with a missing company name and assert that the address is successfully changed.
[Theory]
[DapperAutoData("")]
[DapperAutoData(" ")]
public void ChangeAddress_MissingCompany_ChangeSuccessful(StringCompanyName companyName, AccountEntity account, Address address)
{
    // Arrange
    address.CompanyName = companyName; // address.CompanyName is a string field. Notice that StringCompanyName can be used in place of a string type.

    // Act
    var result = account.ChangeAddress(address);

    // Assert
    result.Items.Should().BeNullOrEmpty();
    account.Address.Should().Be(address);
}
``` 

#### Example of a Test with Injected Services in an Async Class

```
/// This test demonstrates how you would test an class that uses injected interfaces. 
/// The [Frozen] attribute marks the interface as a singleton, so it will be the same instance for all tests.
/// The AccountService class may have many injected services, but they will all be automatically mocked by AutoMoq.
/// This test will only need to deal with the specific interface that effects this test.
[Theory]
[DapperAutoData]
public async Task CreateAsync_CreateDuplicateAccount_ReturnsDuplicateAccountErrorAsync( CreateAccountRequest request,
    [Frozen] Mock<IAccountRepo> repo,
    AccountService service )
{
    // Arrange
    repo.Setup(x => x.DoesEmailExistAsync(request.LoginEmail, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true); // To test this properly we need to ensure the repo reports that the email already exists.

    // Act
    var response = await service.CreateAsync(request, CancellationToken.None);

    // Assert
    response.ErrorCode.Should().Be(ErrorCode.Duplicate);
    response.IsSuccess.Should().BeFalse();
    response.ErrorMessage.Should().Contain(BrokerResponse.DuplicateEmailError);

    repo.Verify(x => x.CreateAsync(It.IsAny<AccountEntity>(), It.IsAny<CancellationToken>()), Times.Never);
}
``` 

#### Testing for synchronous Exceptions
```
/// This test shows how you can test for exceptions in synchronus methods.
[Theory]
[DapperAutoData]
public void Create_InvalidAccount_ThrowsException(CreateAccountRequest request)
{
    // Arrange
    var service = new AccountService();

    // Act
    Action act = () => service.Create(request);

    // Assert
    act.Should().Throw<InvalidAccountException>();
}
``` 

This will assert that calling the `Create` method with the provided `request` will throw an `InvalidAccountException`.

#### Testing for asynchronous Exceptions

```
/// This test shows how you can test for exceptions in asynchronous methods.
[Theory]
[DapperAutoData]
public async Task CreateAsync_InvalidAccount_ThrowsExceptionAsync(CreateAccountRequest request)
{
	// Arrange
	var service = new AccountService();

	// Act
	Func<Task> act = async () => await service.CreateAsync(request, CancellationToken.None);

	// Assert
	await act.Should().ThrowAsync<InvalidAccountException>();
}
```

## Existing Generator Types

The Dapper Testing Library has a variety of built-in types that can be used to specify ranges and types of auto data. These can generally be used interchangeably with the types they are generating for, or you can grab the specific value from the `.Value` attribute.

### String Generators

- `StringParagraph` (string)
- `StringPersonFullName` (string)
- `StringPhoneNumber` (string)
- `StringSentence` (string)
- `StringSsn` (string)
- `StringWord` (string)
- `StringCompanyName` (string)
- `StringEmailTest` (string)
- `StringFirstName` (string)
- `StringInternetUrl` (string)
- `StringInternetUsername` (string)
- `StringCityName` (string)
- `StringCountryName` (string)
- `StringPostalCode` (string)
- `StringStateAbbreviation` (string)
- `StringStateFullName` (string)
- `StringStreetAddress` (string)
- `StringJobTitle` (string)
- `StringProductCategory` (string)
- `StringProductDescription` (string)
- `StringCurrency` (string)
- `StringFileExtension` (string)
- `StringIPAddress` (string)
- `StringHtmlTag` (string)
- `StringPassword` (string)
- `StringGuid` (string)

### Number Generators

- `NumberPositive` (decimal)
- `NumberNegative` (decimal)
- `NumberMoney` (decimal)
- `NumberFraction` (decimal)

### DateTime Generators

- `DateTimeFuture` (DateTime)
- `DateTimeOffsetFuture` (DateTimeOffset)
- `DateTimeOfBirth` (DateTime)
- `DateTimeOffsetOfBirth` (DateTimeOffset)
- `DateTimePast` (DateTime)
- `DateTimeOffsetPast` (DateTimeOffset)
- `CurrentDate` (DateTime)
- `DateSpecialEvent` (DateTime)
- `DateWeekday` (DateTime)
- `DateWeekend` (DateTime)
- `DateLeapYear` (DateTime)
- `DateCustomFormat` (DateTime)

## Community

Feel free to submit issues and enhancement requests. Contributions are welcome!

1.  Fork the repository
2.  Create your feature branch (`git checkout -b feature/myNewFeature`)
3.  Commit your changes (`git commit -m 'Add some feature'`)
4.  Push to the branch (`git push origin feature/myNewFeature`)
5.  Create a new Pull Request

## Acknowledgements

We want to express our gratitude to all the projects that made this library possible.

-   [AutoFixture](https://github.com/AutoFixture/AutoFixture)
-   [Fluent Assertions](https://github.com/fluentassertions/fluentassertions)
-   [Faker.Net](https://github.com/bchavez/Bogus)
-   [AutoMoq](https://github.com/Moq/moq4/wiki/Quickstart)

## License

Dapper Testing Library is released under the MIT License. See the bundled `LICENSE` file for details.