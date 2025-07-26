using System.Runtime.ExceptionServices;
using JetBrains.Annotations;
using System.Threading;

namespace TPLTask1;

/// <summary>
/// Ряд Лейбница: SUM += 1 / (1 + 2 * n) * (-1)^n
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class LeibnizSeriesCalculator
{
    public double Calculate(int startX, int endX, int threadsCount)
    {
        var currentValueLock = new object();
        var currentValue = 0d;

        var rangeX = endX - startX;
        var step = rangeX / threadsCount;
        var threads = new Thread[threadsCount];
        var threadsStatuses = new bool[threadsCount];
        var threadsStatusesLock = new object();

        for (var threadId = 0; threadId < threadsCount; threadId++)
        {
            var offset = threadId * step;

            var thread = new Thread(id =>
            {
                // TODO ???
                // ReSharper disable once AccessToModifiedClosure
                CalculateFormulaRange(offset, offset + step, ref currentValueLock, ref currentValue);

                lock (threadsStatusesLock)
                {
                    threadsStatuses[(int)id!] = true;
                }
            });

            threads[threadId] = thread;
            thread.Start(threadId);
        }

        while (true)
        {
            lock (threadsStatusesLock)
            {
                if (threadsStatuses.All(x => x))
                {
                    break;
                }

                Thread.Sleep(10);
            }
        }

        currentValue = AfterCalculate(currentValue);

        return currentValue;
    }

    private void CalculateFormulaRange(int startX, int endX, ref object sumLockObject, ref double sum)
    {
        var localSum = 0d;

        for (var x = startX; x < endX; x++)
        {
            // TODO Нужен ли lock для объекта, у которого вызывается метод с параметрами?
            localSum += CalculateIteration(x);
        }

        lock (sumLockObject)
        {
            sum += localSum;
        }
    }

    public double CalculateIteration(int x)
    {
        return 1d / (1d + 2d * x) * Math.Pow(-1, x);
    }

    public double AfterCalculate(double endValue)
    {
        return endValue * 4;
    }
}