using JetBrains.Annotations;

namespace TPLTask1.Calculation;

/// <summary>
/// Ряд Лейбница: SUM += 1 / (1 + 2 * n) * (-1)^n
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class LeibnizSeriesCalculator
{
    public LeibnizSeriesCalculator()
    {
        // JIT HEAT

        Calculate(0, 0, 0);
        CalculateFormulaRange(0, 0);
        CalculateIteration(0);
        AfterCalculate(0);
    }

    public double Calculate(long startX, long endX, int threadsCount)
    {
        if (startX == endX || threadsCount == 0)
        {
            return default;
        }

        var rangeX = endX - startX;
        var step = rangeX / threadsCount;
        var threads = new Thread?[threadsCount];
        var values = new double[threadsCount];
        var threadsStatuses = new bool[threadsCount];

        for (var threadId = 1; threadId < threadsCount; threadId++)
        {
            var offset = threadId * step;

            var thread = new Thread(id =>
            {
                var _threadId = (int)id!;

                var sum = CalculateFormulaRange(offset, offset + step);

                values[_threadId] = sum;
                threadsStatuses[_threadId] = true;
            });

            threads[threadId] = thread;
            thread.Start(threadId);
        }
        
        values[0] = CalculateFormulaRange(0, step);
        threadsStatuses[0] = true;

        foreach (var thread in threads)
        {
            thread?.Join();
        }

        var currentValue = values.Sum();

        currentValue = AfterCalculate(currentValue);

        return currentValue;
    }

    private double CalculateFormulaRange(long startX, long endX)
    {
        var localSum = 0d;

        for (var x = startX; x < endX; x++)
        {
            localSum += CalculateIteration(x);
        }

        return localSum;
    }

    public double CalculateIteration(long x)
    {
        return ((x & 1) == 0 ? 1d : -1d) / (1d + 2d * x);
    }

    public double AfterCalculate(double endValue)
    {
        return endValue * 4;
    }
}