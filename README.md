# Dapper System Integration Testing Library

As a fan of TDD, BDD, and anonymous testing I've made heavy use of AutoFixture, AutoMoq, and Fluent Assertions in my projects since around 2015. I've been lucky enough to work with a few other developers across these companies, and we found we kept needing to implement the same patterns and utilites every time we moved to a new project. We even lost some work and knowledge along the way. This library is an attempt to consolidate all of that knowledge into a single library that can be used across all of our projects. It's a work in progress, but I hope it can be useful to others as well.

DapperAutoData (yes, unrelated to Dapper ORM. I was using the name first!) is a library that simplifies the process of setting up tests and generating data for your tests. It is built on top of AutoFixture, AutoMoq, and Fluent Assertions, and provides a set of attributes that you can use to automatically generate data for your tests. 

It sets up patterns for creating data generators, customizing data generation, and testing for exceptions. It also provides a set of built-in data generators that you can use to generate specific types of data for your tests. You can also create your own data generators to generate custom data for your tests.

To be clear, I in no way intend for this to be seen as novel work or take credit for anything done by Mark Seemann, or any of the other developers who have worked on these projects. This is simply a consolidation of the work they have done, and a way to make it easier to use across all of our projects, with the hope that it might provide some value to others. If any of the authors of the libraries used here have ANY concerns with this use, please reach out to me and I'll do my best to rectify it, or even take this repo down.

## Features

-   **AutoFixture** to generate anonymous data for your tests.
-   **AutoMoq** for automocking in your unit tests.
-   **Fluent Assertions** for more readable assertions.

## Useful Links

-   [AutoFixture Documentation](https://github.com/AutoFixture/AutoFixture/wiki)
-   [Fluent Assertions Documentation](https://fluentassertions.com/documentation)
-   [Faker.Net Documentation](https://github.com/bchavez/Bogus)
-   [AutoMoq Documentation](https://github.com/Moq/moq4/wiki/Quickstart)

## Installation

In package manager console run the command: `Install-Package DapperAutoData.SystemIntegrationUtilities` 

The library is distributed as a nuget package. After installing the package all you need to do is add the `DapperServiceClientBase` as base class to you client Implementation. This base class wrapper for http client and include TokenProvider if available. 


## How to Use

### ExampleService

```
public interface IExampleService
{
    Task<ServiceResponse<ExampleInfo>> CreateExampleAsync(CreateExampleModel model, CancellationToken token);
}

public class ExampleServiceClient : DapperServiceClientBase, IExampleService
{
    public ExampleServiceClient(HttpClient httpclient, JsonSerializerOptions jsonSerializerOptions, TokenProvider tokenProvider) : base(httpclient, jsonSerializerOptions, tokenProvider)
    {
    }

    public async Task<ServiceResponse<ExampleInfo>> ChangeNameAsync(string id, ChangeExampleNameModel model, CancellationToken token)
    {
        var endpoint = $"api/Example/{id}/ChangeName";
        var response = await this.Put<ChangeExampleNameModel, ServiceResponse<ExampleInfo>>(endpoint, model, null, token).ConfigureAwait(false);

        return response;
    }
}

```

### Creating a Client Generator and TokenProvider

Next, you need to create a Client Generator. This is a class that will generate a client for you. 
Here is also an example on how to pregenerate a token for the token provider and include the token provider in the client generator. 

```
public class ClientServiceGenerators : IDataGenerator
{
    private static string _baseUri = "https://api.example.com";

    private TokenProvider _tokenProvider;

    public void RegisterGenerators(IFixture fixture)
    {
        RegisterJsonSerializer(fixture);
    
        var preGeneratedTokenProvider = new PreGeneratedTokenProvider(fixture);
        _tokenProvider = preGeneratedTokenProvider.tokenProvider;

        RegisterExampleService(fixture);
    }

    private void RegisterJsonSerializer(IFixture fixture)
    {
        fixture.Register(() =>
        {

            JsonSerializerOptions jsonSerializerOptions = new()
            {
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            return jsonSerializerOptions;
        });
    }
    private void RegisterExampleService(IFixture fixture)
	{
		fixture.Register(() =>
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(_baseUri)
			};

			return new ExampleServiceClient(httpClient, fixture.Create<JsonSerializerOptions>(), _tokenProvider);
		});
	}

}
```


```
public class PreGeneratedTokenProvider
{
    internal IAuthenticationServiceClient client;
    public TokenProvider? tokenProvider;
    internal IFixture fixture;
    public PreGeneratedTokenProvider(IFixture fixture)
    {
        tokenProvider = TokenProviderTrackerCollection.GeTokenProviderSync(fixture);
    }
}

public class TokenProviderTrackerCollection : IDisposable
{
    private static TokenProvider? tokenProvider = null;
    private static IAuthenticationServiceClient? client;
    public void Dispose()
    {
        tokenProvider = null;
        client = null;
    }
    
    public static TokenProvider GeTokenProviderSync(IFixture fixture)
    {
        if (client == null)
        {
            client = fixture.Create<AuthenticationServiceClient>();
        }

        if (tokenProvider == null)
        {
            var authenticationResponse = client.AuthenticateSync();
            tokenProvider = new TokenProvider
            {
                AccessToken = authenticationResponse.AccessToken
            };
        }
        return tokenProvider;
    }
}

```


#### Example of System integration test

```
public class ExampleServiceClientTests
{

    [Theory]
    [DapperAutoData()]
    public async Task CreateExampleAsync_ValidData_Success(
        CreateExampleModel request,
        IExampleService client)
    {
        // Arrange


        // Act
        var response = await client.CreateExampleAsync(request, CancellationToken.None).ConfigureAwait(true);

        // Assert
        response.Data.Name.Should().Be(request.Name);
        response.Data.UserId.Should().Be(request.UserId);
    }
}

```

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