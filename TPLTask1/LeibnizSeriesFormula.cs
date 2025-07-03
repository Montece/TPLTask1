using TPLTask1.Calculation;

namespace TPLTask1;

/// <summary>
/// Ряд Лейбница: SUM += 1 / (1 + 2 * n) * (-1)^n
/// </summary>
internal sealed class LeibnizSeriesFormula : IFormula
{
    public double BeforeCalculate(double startValue)
    {
        return startValue;
    }

    public double Calculate(int x)
    {
        return 1d / (1d + 2d * x) * Math.Pow(-1, x);
    }

    public double AfterCalculate(double endValue)
    {
        return endValue * 4;
    }
}