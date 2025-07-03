using JetBrains.Annotations;

namespace TPLTask1.Calculation;

[UsedImplicitly]    
public interface IFormula
{
    double BeforeCalculate(double startValue);

    double Calculate(int x);

    double AfterCalculate(double endValue);
}