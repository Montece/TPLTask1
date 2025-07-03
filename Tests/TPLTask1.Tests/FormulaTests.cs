using Moq;
using TPLTask1.Calculation;
using Xunit;

namespace TPLTask1.Tests;

public class FormulaTests
{
    [Theory]
    [InlineData(10, 11)]
    [InlineData(-10, -9)]
    [InlineData(0, 1)]
    [InlineData(-9999999, -9999998)]
    [InlineData(9999999, 10000000)]
    public void Calculate_XValue_ShouldReturnIncrementX(int value, int expectedValue)
    {
        var mock = new Mock<IFormula>();
        mock.Setup(f => f.Calculate(It.IsAny<int>()))
            .Returns((int x) => x + 1);

        var formula = mock.Object;

        var actualValue = formula.Calculate(value);

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(10d, 5d)]
    [InlineData(-10d, -5d)]
    [InlineData(0d, 0d)]
    [InlineData(-5000d, -2500d)]
    [InlineData(5000d, 2500d)]
    public void BeforeCalculate_StartValue_ShouldReturnDivideByTwo(double value, double expectedValue)
    {
        var mock = new Mock<IFormula>();
        mock.Setup(f => f.BeforeCalculate(It.IsAny<double>()))
            .Returns((double x) => x / 2);

        var formula = mock.Object;

        var actualValue = formula.BeforeCalculate(value);

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(10d, 5d)]
    [InlineData(-10d, -5d)]
    [InlineData(0d, 0d)]
    [InlineData(-5000d, -2500d)]
    [InlineData(5000d, 2500d)]
    public void AfterCalculate_StartValue_ShouldReturnDivideByTwo(double value, double expectedValue)
    {
        var mock = new Mock<IFormula>();
        mock.Setup(f => f.AfterCalculate(It.IsAny<double>()))
            .Returns((double x) => x / 2);

        var formula = mock.Object;

        var actualValue = formula.AfterCalculate(value);

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(10d, 100d)]
    [InlineData(-10d, 100d)]
    [InlineData(0d, 0d)]
    [InlineData(-50d, 2500d)]
    [InlineData(50d, 2500d)]
    public void AfterCalculate_StartValue_ShouldReturnPowOfTwo(double value, double expectedValue)
    {
        var mock = new Mock<IFormula>();
        mock.Setup(f => f.AfterCalculate(It.IsAny<double>()))
            .Returns((double x) => Math.Pow(x, 2));

        var formula = mock.Object;

        var actualValue = formula.AfterCalculate(value);

        Assert.Equal(expectedValue, actualValue);
    }
}