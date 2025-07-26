using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace TPLTask1;

[UsedImplicitly]
internal sealed class Program
{
    public static void Main(string[] args)
    {
        Console.Title = Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";

        const double expectedPi = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067d;

        var calculator = new LeibnizSeriesCalculator();
        var stopwatch = new Stopwatch();
        double pi;

        Console.WriteLine(new string('=', 88));

        for (var i = 1; i <= 20; i++)
        {
            stopwatch.Restart();
            pi = calculator.Calculate(0, 100_000_000, i);
            stopwatch.Stop();

            Console.WriteLine($"Threads: {i.ToString(),2}, Time: {stopwatch.ElapsedMilliseconds:F} ms, Pi: {pi:F20}, Delta: {Math.Abs(pi - expectedPi):F20}");
        }
        
        Console.WriteLine(new string('=', 88));
        
        var calculatorInfinity = new LeibnizSeriesCalculatorInfinity();

        stopwatch.Restart();
        calculatorInfinity.BeginCalculate(10);

        Console.WriteLine("Calculating Pi... Press ENTER to cancel...");

        Console.ReadLine();

        pi = calculatorInfinity.EndCalculate();
        stopwatch.Stop();

        var time = $"Time: {stopwatch.ElapsedMilliseconds:F}";
        Console.WriteLine($"{time,25} ms, Pi: {pi:F20}, Delta: {Math.Abs(pi - expectedPi):F20}");
        
        Console.WriteLine();
        Console.WriteLine("Press ENTER to exit...");
        Console.ReadLine();
    }
}