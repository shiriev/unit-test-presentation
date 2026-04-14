namespace BookingLogic;

/// <summary>
/// Сервис для проверки возможности занятия места/времени
/// </summary>
public class BookingValidator
{
    /// <summary>
    /// Проверяет, можно ли занять интервал
    /// </summary>
    /// <param name="requested">Запрашиваемый интервал</param>
    /// <param name="bookedIntervals">Список уже занятых интервалов</param>
    /// <param name="minGapMinutes">Минимальный промежуток между посещениями (в минутах)</param>
    /// <returns>True, если интервал свободен и соблюдены все условия</returns>
    public bool CanBook(TimeInterval requested, List<TimeInterval> bookedIntervals, int minGapMinutes)
    {
        if (requested is null)
            throw new ArgumentNullException(nameof(requested));
        
        if (minGapMinutes < 0)
            throw new ArgumentException("Min gap cannot be negative", nameof(minGapMinutes));

        // Если нет занятых интервалов, сразу возвращаем true
        if (bookedIntervals == null || !bookedIntervals.Any())
            return true;

        // Проверяем пересечения с учетом минимального промежутка
        foreach (var booked in bookedIntervals)
        {
            // Расширяем занятый интервал на минимальный промежуток
            var expandedBookedStart = booked.Start.AddMinutes(-minGapMinutes);
            var expandedBookedEnd = booked.End.AddMinutes(minGapMinutes);
            var expandedBooked = new TimeInterval(expandedBookedStart, expandedBookedEnd);

            // Если запрашиваемый интервал пересекается с расширенным занятым - место нельзя занять
            if (requested.OverlapsWith(expandedBooked))
                return false;
        }

        return true;
    }
}