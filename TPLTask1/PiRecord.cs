namespace TPLTask1;

public class PiRecord
{
    public double Pi { get; }
    public long TimeInMs { get; }
    public int ThreadsCount { get; }
    public double DeltaPi { get; }

    public PiRecord(double pi, long timeInMs, int threadsCount, double deltaPi)
    {
        Pi = pi;
        TimeInMs = timeInMs;
        ThreadsCount = threadsCount;
        DeltaPi = deltaPi;
    }
}