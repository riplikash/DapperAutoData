using DapperAutoData;

namespace Examples;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Examples.ExampleClasses;
using FluentAssertions;
using Moq;
using Xunit;

// Assuming you have the necessary using statements for your project

public class PersonServiceTests
{
    [Theory]
    [DapperAutoData]
    public async Task GetByIdAsync_ShouldReturnServiceResponseWithPerson_WhenPersonExists(
        [Frozen] Mock<IPersonRepository> mockRepository,
        Person person,
        PersonService service)
    {
        // Arrange
        mockRepository.Setup(repo => repo.GetByIdAsync(person.Id))
            .ReturnsAsync(person);

        // Act
        var response = await service.GetByIdAsync(person.Id);

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(person);
        response.ErrorMessage.Should().BeNull();

        mockRepository.Verify(repo => repo.GetByIdAsync(person.Id), Times.Once);
    }

    [Theory]
    [DapperAutoData]
    public async Task GetByIdAsync_ShouldReturnServiceResponseWithError_WhenPersonDoesNotExist(
        [Frozen] Mock<IPersonRepository> mockRepository,
        int personId,
        PersonService service)
    {
        // Arrange
        mockRepository.Setup(repo => repo.GetByIdAsync(personId))
            .ReturnsAsync((Person)null);

        // Act
        var response = await service.GetByIdAsync(personId);

        // Assert
        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().NotBeNullOrEmpty();

        mockRepository.Verify(repo => repo.GetByIdAsync(personId), Times.Once);
    }

    // More test cases for other methods...

    [Theory]
    [DapperAutoData]
    public async Task DeleteAsync_ShouldReturnServiceResponseWithSuccess_WhenDeletionSucceeds(
        [Frozen] Mock<IPersonRepository> mockRepository,
        int personId,
        PersonService service)
    {
        // Arrange
        mockRepository.Setup(repo => repo.DeleteAsync(personId))
            .Returns(Task.CompletedTask);

        // Act
        var response = await service.DeleteAsync(personId);

        // Assert
        response.Success.Should().BeTrue();
        response.ErrorMessage.Should().BeNull();

        mockRepository.Verify(repo => repo.DeleteAsync(personId), Times.Once);
    }
}
