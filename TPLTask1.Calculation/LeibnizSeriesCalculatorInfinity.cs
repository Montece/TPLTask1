using JetBrains.Annotations;

namespace TPLTask1.Calculation;

/// <summary>
/// Ряд Лейбница: SUM += 1 / (1 + 2 * n) * (-1)^n
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class LeibnizSeriesCalculatorInfinity : IDisposable
{
    private CancellationTokenSource _cts = new();
    private double _currentValue;
    private object _currentValueLock = new();
    private bool[]? _threadsStatuses;
    private object _threadsStatusesLock = new();

    public void BeginCalculate(int threadsCount)
    {
        var stepX = 100;
        var startX = 0;

        new Thread(() =>
        {
            while (!_cts.IsCancellationRequested)
            {
                var threads = new Thread[threadsCount];

                lock (_threadsStatusesLock)
                {
                    _threadsStatuses = new bool[threadsCount];
                }

                for (var threadId = 0; threadId < threadsCount; threadId++)
                {
                    var step = stepX / threadsCount;
                    var offset = startX + threadId * step;

                    var thread = new Thread(id =>
                    {
                        CalculateFormulaRange(offset, offset + step, ref _currentValueLock, ref _currentValue);

                        lock (_threadsStatusesLock)
                        {
                            _threadsStatuses[(int)id!] = true;
                        }
                    });

                    threads[threadId] = thread;
                    thread.Start(threadId);
                }

                while (true)
                {
                    lock (_threadsStatusesLock)
                    {
                        if (_threadsStatuses.All(x => x))
                        {
                            break;
                        }

                        Thread.Sleep(10);
                    }
                }

                startX += stepX;
            }
        }).Start();
    }

    public double EndCalculate()
    {
        _cts.Cancel();

        while (true)
        {
            lock (_threadsStatusesLock)
            {
                if (_threadsStatuses!.All(x => x))
                {
                    break;
                }

                Thread.Sleep(10);
            }
        }

        lock (_currentValueLock)
        {
            _currentValue = AfterCalculate(_currentValue);

            return _currentValue;
        }
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