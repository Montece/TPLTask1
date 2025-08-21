using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace TPLTask1.Calculation;

/// <summary>
/// Ряд Лейбница: SUM += 1 / (1 + 2 * n) * (-1)^n
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class LeibnizSeriesCalculatorInfinity : IDisposable
{
    private CancellationTokenSource _cts = new();
    private double _currentValue;
    private Thread _calculateThread;

    public LeibnizSeriesCalculatorInfinity()
    {
        _calculateThread = new(threadsCountObject =>
        {
            var threadsCount = (int)threadsCountObject!;

            var startX = 0;
            var stepX = 100;

            while (!_cts.IsCancellationRequested)
            {
                var threads = new Thread?[threadsCount];
                var values = new double[threadsCount];

                var chunk = (stepX + threadsCount - 1) / threadsCount;
                var chunks = Partitioner.Create(startX, startX + stepX, chunk).GetDynamicPartitions().ToList();

                for (var threadId = 0; threadId < chunks.Count; threadId++)
                {
                    var startInfo = new StartInfo
                    {
                        ThreadId = threadId,
                        StartLocalX = chunks[threadId].Item1,
                        EndLocalX = chunks[threadId].Item2 - 1
                    };

                    var thread = new Thread(__startInfo =>
                    {
                        var _startInfo = (StartInfo)__startInfo!;

                        var sum = CalculateFormulaRange(_startInfo.StartLocalX, _startInfo.EndLocalX);

                        values[_startInfo.ThreadId] = sum;
                    });

                    threads[threadId] = thread;
                    thread.Start(startInfo);
                }

                foreach (var thread in threads)
                {
                    thread?.Join();
                }

                _currentValue += values.Sum();

                startX += stepX;
            }
        });
    }

    public void BeginCalculate(int threadsCount)
    {
        _calculateThread.Start(threadsCount);
    }

    public double EndCalculate()
    {
        _cts.Cancel();
        _calculateThread.Join();

        _currentValue = AfterCalculate(_currentValue);

        return _currentValue;
    }

    private double CalculateFormulaRange(long startX, long endX)
    {
        var localSum = 0d;

        for (var x = startX; x <= endX; x++)
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

    public void Dispose()
    {
        _cts.Dispose();
    }
}

public struct StartInfo
{
    public int ThreadId;
    public long StartLocalX;
    public long EndLocalX;
}