namespace BookingLogic;

/// <summary>
/// Представляет интервал времени
/// </summary>
public class TimeInterval
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TimeInterval(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start must be less than End");
        
        Start = start;
        End = end;
    }

    public bool OverlapsWith(TimeInterval other)
    {
        return Start < other.End && End > other.Start;
    }
}
