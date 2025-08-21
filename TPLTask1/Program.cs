using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using CsvHelper;
using JetBrains.Annotations;
using TPLTask1.Calculation;

namespace TPLTask1;

[UsedImplicitly]
internal sealed class Program
{
    private const double PI = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067d;

    public static void Main(string[] args)
    {
        Console.Title = Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";

        var piRecords = new List<PiRecord>();

        Console.WriteLine(new string('=', 88));

        Console.WriteLine("Calculating...");

        /*for (var i = 2; i <= 1024; i *= 2)
        {
            var piRecord = CalculatePi(i);

            piRecords.Add(piRecord);
        }*/

        /*for (var i = 1; i <= 100; i++)
        {
            var piRecord = CalculatePi(i);

            piRecords.Add(piRecord);
        }

        piRecords = piRecords.OrderBy(x => x.TimeInMs).ToList();

        foreach (var piRecord in piRecords)
        {
            Console.WriteLine($"Threads: {piRecord.ThreadsCount,4} | Time: {piRecord.TimeInMs,5} ms | PI: {piRecord.Pi:F20} | Delta: {piRecord.DeltaPi:F20}");
        }

        using (var writer = new StreamWriter("pi.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(piRecords);
        }
        
        Console.WriteLine(new string('=', 88));
        
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
        */

        // Итератор
        
        /*using var calculatorInfinity = new LeibnizSeriesCalculatorInfinity();
        var stopwatch = new Stopwatch();

        stopwatch.Restart();
        calculatorInfinity.BeginCalculate(17);

        Console.WriteLine("Calculating PI... Press ENTER to cancel...");
        Console.ReadLine();

        var pi_ = calculatorInfinity.EndCalculate();
        stopwatch.Stop();

        var time = $"Time: {stopwatch.ElapsedMilliseconds:F}";
        Console.WriteLine($"{time,25} ms, PI: {pi_:F20}, Delta: {Math.Abs(pi_ - PI):F20}");
        */

        for (var i = 1; i <= 30; i++)
        {
            using var calculatorInfinity = new LeibnizSeriesCalculatorInfinity();
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            calculatorInfinity.BeginCalculate(i);

            Thread.Sleep(10 * 1000);

            var pi_ = calculatorInfinity.EndCalculate();
            stopwatch.Stop();

            var time = $"Time: {stopwatch.ElapsedMilliseconds:F}";
            Console.WriteLine($"{time,25} ms, PI: {pi_:F20}, Delta: {Math.Abs(pi_ - PI):F20}");
        }

        Console.WriteLine();
        Console.WriteLine("Press ENTER to exit...");
        Console.ReadLine();
    }

    private static PiRecord CalculatePi(int threadsCount)
    {
        using var process = new Process();

        process.StartInfo = new()
        {
            FileName = @"..\..\..\..\TPLTask1.Executable\bin\Debug\net8.0\TPLTask1.Executable.exe",
            ArgumentList = { threadsCount.ToString() },
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        process.Start();

        var outputs = process.StandardOutput.ReadToEnd().Split(Environment.NewLine).ToList();
        outputs.RemoveAll(x => x.Equals(string.Empty));

        var pi = double.Parse(outputs[0]);
        var timeInMs = long.Parse(outputs[1]);
        var piRecord = new PiRecord(pi, timeInMs, threadsCount, Math.Abs(pi - PI));
        
        return piRecord;
    }
}