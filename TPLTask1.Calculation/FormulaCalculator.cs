using JetBrains.Annotations;
using TPLTask.Utility;

namespace TPLTask1.Calculation;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class Formula
{
    private readonly IFormula _formula;

    public Formula(IFormula formula)
    {
        Guard.NotNull(nameof(formula), formula);

        _formula = formula;
    }

    public async Task<double> CalculateAsync(int startX, int endX, int threadsCount)
    {
        var currentValueLock = new object();
        var currentValue = 0d;

        var rangeX = endX - startX;
        var step = rangeX / threadsCount;
        var tasks = new Task[threadsCount];

        currentValue = _formula.BeforeCalculate(currentValue);

        for (var threadId = 0; threadId < threadsCount; threadId++)
        {
            var offset = threadId * step;

            // TODO Correct?
            // ReSharper disable once AccessToModifiedClosure
            tasks[threadId] = Task.Run(() => CalculateFormulaRange(offset, offset + step, currentValueLock, ref currentValue));
        }

        await Task.WhenAll(tasks);
        
        currentValue = _formula.AfterCalculate(currentValue);

        return currentValue;
    }

    public async Task<double> CalculateAsync(int startX, CancellationToken cancellationToken)
    {
        var currentValue = 0d;

        currentValue = _formula.BeforeCalculate(currentValue);
    
        currentValue += await Task.Run(delegate
        {
            var x = 0;
            var localCurrentValue = 0d;

            while (!cancellationToken.IsCancellationRequested)
            {
                localCurrentValue += _formula.Calculate(x);
                x++;
            }

            return localCurrentValue;

        }, cancellationToken);
        
        currentValue = _formula.AfterCalculate(currentValue);

        return currentValue;
    }

    private void CalculateFormulaRange(int startX, int endX, object sumLockObject, ref double sum)
    {
        var localSum = 0d;

        for (var x = startX; x < endX; x++)
        {
            localSum += _formula.Calculate(x); // TODO !!! lock для _formula не нужен?
        }

        lock (sumLockObject)
        {
            sum += localSum;
        }
    }
}