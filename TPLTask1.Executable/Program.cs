using System.Diagnostics;
using TPLTask1.Calculation;

var parameters = Environment.GetCommandLineArgs();

if (parameters.Length == 1)
{
    return;
}

var threadsCount = int.Parse(parameters[1]);

var calculator = new LeibnizSeriesCalculator();
var stopwatch = new Stopwatch();

stopwatch.Restart();
var pi = calculator.Calculate(0, 128_000_000, threadsCount);
stopwatch.Stop();
var time = stopwatch.ElapsedMilliseconds;

Console.WriteLine(pi);
Console.WriteLine(time);