
# Dapper Testing Library

This is a flexible, robust testing library that integrates a variety of powerful testing frameworks and tools. The library brings together AutoFixture, AutoMoq, Fluent Assertions, and Moq. It also includes a set of custom data generators for more targeted data generation and provides a pattern for adding new custom data generators.

## Features

-   **AutoFixture** to generate anonymous data for your tests.
-   **AutoMoq** for automocking in your unit tests.
-   **Fluent Assertions** for more readable assertions.
-   **Moq** for manual mocking.
-   **Custom Data Generators** for generating specific types of data.

## Installation

The library is distributed as a nuget package. After installing the package, the files `DataGeneratorInstaller` and `AutoMoqDataAttribute.cs` will be created.

shellCopy code

`Install-Package DapperTestingLibrary` 

The installer will attempt to install any data generators in the assembly. A data generator is a class that implements the `IDataGenerator` interface.

## How to Use

### Creating a Data Generator

csharpCopy code

`public class AssistantGenerator : IDataGenerator
{
    public void RegisterGenerators(IFixture fixture)
    {
        fixture.Register(() => GenerateAssistant(fixture));
    }

    private static Assistant GenerateAssistant(IFixture fixture) =>
        new()
        {
            ContactInfo = fixture.Create<ContactInfo>(),
            Name = Faker.Name.FullName()
        };
}` 

### Using Dapper Testing Library in your Tests

Here are some examples of tests using the Dapper Testing Library:

#### Example of a Unit Test

csharpCopy code

`[Theory]
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
}` 

#### Example of a Theory Driven Test

csharpCopy code

`[Theory]
[DapperAutoData("")]
[DapperAutoData(" ")]
public void ChangeAddress_MissingCompany_ChangeSuccessful(string companyName, AccountEntity account, Address address)
{
    // Arrange
    address.CompanyName = companyName;

    // Act
    var result = account.ChangeAddress(address);

    // Assert
    result.Items.Should().BeNullOrEmpty();
    account.Address.Should().Be(address);
}` 

#### Example of a Test with Injected Services in an Async Class

csharpCopy code

`[Theory]
[DapperAutoData]
public async Task CreateAsync_CreateDuplicateAccount_ReturnsDuplicateAccountErrorAsync( CreateAccountRequest request,
    [Frozen] Mock<IAccountRepo> repo,
    AccountService service )
{
    // Arrange
    repo.Setup(x => x.DoesEmailExistAsync(request.LoginEmail, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    // Act
    var response = await service.CreateAsync(request, CancellationToken.None);

    // Assert
    response.ErrorCode.Should().Be(ErrorCode.Duplicate);
    response.IsSuccess.Should().BeFalse();
    response.ErrorMessage.Should().Contain(BrokerResponse.DuplicateEmailError);

    repo.Verify(x => x.CreateAsync(It.IsAny<AccountEntity>(), It.IsAny<CancellationToken>()), Times.Never);
}` 

## Useful Links

-   [AutoFixture Documentation](https://github.com/AutoFixture/AutoFixture/wiki)
-   [Fluent Assertions Documentation](https://fluentassertions.com/documentation)
-   [Faker.Net Documentation](https://github.com/bchavez/Bogus)
-   [AutoMoq Documentation]([https://github.com/Moq/moq4/wiki/Quick](https://github.com/Moq/moq4/wiki/Quickstart)

## Testing for Exceptions

While there isn't a specific example included here for testing exceptions, it's worth mentioning that Fluent Assertions has excellent support for assertions on raised exceptions. Here's a simple example:

csharpCopy code

`[Theory]
[DapperAutoData]
public void Create_InvalidAccount_ThrowsException(CreateAccountRequest request)
{
    // Arrange
    var service = new AccountService();

    // Act
    Action act = () => service.Create(request);

    // Assert
    act.Should().Throw<InvalidAccountException>();
}` 

This will assert that calling the `Create` method with the provided `request` will throw an `InvalidAccountException`.

## Customizing AutoFixture

In some cases, you may need to customize AutoFixture to create objects in a certain way. Here's an example of how you can do that:

csharpCopy code

`public class CustomAutoDataAttribute : AutoDataAttribute
{
    public CustomAutoDataAttribute() : base(() => 
        new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}` 

This creates a new AutoDataAttribute that you can use in your tests to create objects with AutoMoq. You can further customize this to fit your needs.

## Extending the Library

The Dapper Testing Library is designed to be extensible. You can create your own data generators by implementing the `IDataGenerator` interface, and register them using the `RegisterGenerators` method. You can also customize the way that AutoFixture creates your objects, as shown in the example above.

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