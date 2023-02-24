using FluentAssertions;
using DistanceCalculator.Evaluator.Enums;
using DistanceCalculator.Evaluator.Implementations;

namespace DistanceCalculator.UnitTests;

public class DistanceCalculatorServiceTests
{
    [Fact]
    public async Task Calculate_TSE_ALA_In_Kilometers_ShouldBe_951()
    {
        // Arrange
        var calculator = new IataDistanceCalculator();

        // Act
        var result = calculator.Calculate(51.027811, 71.461199, 43.346652, 77.011455, DistanceUnit.KM);
        var roundedResult = Math.Round(result);

        // Assert
        // assertion result was taken from https://www.airportdistancecalculator.com/flight-tse-to-ala.html#.Y_XrG2BBzb0
        const double assertResult = 951;
        roundedResult.Should().Be(assertResult);
    }

    [Fact]
    public async Task Calculate_TSE_ALA_In_Kilometers_ShouldBe_591()
    {
        // Arrange
        var calculator = new IataDistanceCalculator();

        // Act
        var result = calculator.Calculate(51.027811, 71.461199, 43.346652, 77.011455, DistanceUnit.MI);
        var roundedResult = Math.Round(result);

        // Assert
        // assertion result was taken from https://www.airportdistancecalculator.com/flight-tse-to-ala.html#.Y_XrG2BBzb0
        const double assertResult = 591;
        roundedResult.Should().Be(assertResult);
    }
}