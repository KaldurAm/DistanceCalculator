using DistanceCalculator.ArchitectureTests.Common.Constants;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace DistanceCalculator.ArchitectureTests;

public class ProjectDependencyTests
{
    [Fact]
    public void Core_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Api.Controllers.BaseController).Assembly;

        var otherProjects = new[] { ProjectNamespace.Api, ProjectNamespace.Infrastructure, };

        // Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAll(otherProjects).GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnApi()
    {
        // Arrange
        var assembly = typeof(Infrastructure.ServiceCollectionExtensions).Assembly;

        var otherProjects = new[] { ProjectNamespace.Api, };

        // Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAll(otherProjects).GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_HaveDependencyOnMediatR()
    {
        // Arrange
        var assembly = typeof(Api.Controllers.BaseController).Assembly;

        // Act
        var testResult = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOn(LibraryNamespace.MediatR)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}