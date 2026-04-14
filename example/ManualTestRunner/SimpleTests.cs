using BookingLogic;

namespace ManualTestRunner;

/// <summary>
/// Простые тесты без использования тестового фреймворка
/// </summary>
public class SimpleTests
{
    // private int _passedTests = 0;
    // private int _failedTests = 0;

    public void RunAllTests()
    {
        Console.WriteLine("=== Running Simple Tests (No Test Runner) ===\n");
        
        Test_NoBookedIntervals_ShouldReturnTrue();
        Test_NoOverlap_ShouldReturnTrue();
        Test_ExactOverlap_ShouldReturnFalse();
        Test_PartialOverlap_ShouldReturnFalse();
        Test_RespectsMinGap_StartGap_ShouldReturnTrue();
        Test_RespectsMinGap_EndGap_ShouldReturnTrue();
        Test_ViolatesMinGap_Start_ShouldReturnFalse();
        Test_ViolatesMinGap_End_ShouldReturnFalse();
        Test_MultipleBookedIntervals_NoOverlap_ShouldReturnTrue();
        Test_MultipleBookedIntervals_Overlap_ShouldReturnFalse();
        Test_BookedIntervalsNull_ShouldReturnTrue();
        Test_RequestedNull_ShouldThrowException();
        Test_NegativeMinGap_ShouldThrowException();

        // Console.WriteLine($"\n=== Results: {_passedTests} passed, {_failedTests} failed ===");
    }

    public void Test_NoBookedIntervals_ShouldReturnTrue()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var result = validator.CanBook(requested, new List<TimeInterval>(), 30);
        AssertHelper.Assert(result == true, "NoBookedIntervals_ShouldReturnTrue");
    }

    public void Test_NoOverlap_ShouldReturnTrue()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 14, 0, 0),
            new DateTime(2024, 1, 1, 15, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 10, 0, 0),
                new DateTime(2024, 1, 1, 11, 0, 0)
            ),
            new TimeInterval(
                new DateTime(2024, 1, 1, 16, 0, 0),
                new DateTime(2024, 1, 1, 17, 0, 0)
            )
        };
        
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == true, "NoOverlap_ShouldReturnTrue");
    }

    public void Test_ExactOverlap_ShouldReturnFalse()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 10, 0, 0),
                new DateTime(2024, 1, 1, 11, 0, 0)
            )
        };
        
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == false, "ExactOverlap_ShouldReturnFalse");
    }

    public void Test_PartialOverlap_ShouldReturnFalse()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 9, 30, 0),
            new DateTime(2024, 1, 1, 10, 30, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 10, 0, 0),
                new DateTime(2024, 1, 1, 11, 0, 0)
            )
        };
        
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == false, "PartialOverlap_ShouldReturnFalse");
    }

    public void Test_RespectsMinGap_StartGap_ShouldReturnTrue()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 8, 0, 0),
                new DateTime(2024, 1, 1, 9, 0, 0)
            )
        };
        
        // Зазор 60 минут между 9:00 и 10:00, минимальный промежуток 30 минут - должно быть OK
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == true, "RespectsMinGap_StartGap_ShouldReturnTrue");
    }

    public void Test_RespectsMinGap_EndGap_ShouldReturnTrue()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 12, 0, 0),
                new DateTime(2024, 1, 1, 13, 0, 0)
            )
        };
        
        // Зазор 60 минут между 11:00 и 12:00, минимальный промежуток 30 минут - должно быть OK
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == true, "RespectsMinGap_EndGap_ShouldReturnTrue");
    }

    public void Test_ViolatesMinGap_Start_ShouldReturnFalse()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 9, 15, 0),
                new DateTime(2024, 1, 1, 9, 45, 0)
            )
        };
        
        // Зазор только 15 минут между 9:45 и 10:00, а нужно минимум 30 минут
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == false, "ViolatesMinGap_Start_ShouldReturnFalse");
    }

    public void Test_ViolatesMinGap_End_ShouldReturnFalse()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 11, 15, 0),
                new DateTime(2024, 1, 1, 12, 0, 0)
            )
        };
        
        // Зазор только 15 минут между 11:00 и 11:15, а нужно минимум 30 минут
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == false, "ViolatesMinGap_End_ShouldReturnFalse");
    }

    public void Test_MultipleBookedIntervals_NoOverlap_ShouldReturnTrue()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 13, 0, 0),
            new DateTime(2024, 1, 1, 14, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(new DateTime(2024, 1, 1, 9, 0, 0), new DateTime(2024, 1, 1, 10, 0, 0)),
            new TimeInterval(new DateTime(2024, 1, 1, 11, 0, 0), new DateTime(2024, 1, 1, 12, 0, 0)),
            new TimeInterval(new DateTime(2024, 1, 1, 15, 0, 0), new DateTime(2024, 1, 1, 16, 0, 0))
        };
        
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == true, "MultipleBookedIntervals_NoOverlap_ShouldReturnTrue");
    }

    public void Test_MultipleBookedIntervals_Overlap_ShouldReturnFalse()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 11, 30, 0),
            new DateTime(2024, 1, 1, 12, 30, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(new DateTime(2024, 1, 1, 10, 0, 0), new DateTime(2024, 1, 1, 11, 0, 0)),
            new TimeInterval(new DateTime(2024, 1, 1, 12, 0, 0), new DateTime(2024, 1, 1, 13, 0, 0))
        };
        
        var result = validator.CanBook(requested, bookedIntervals, 30);
        AssertHelper.Assert(result == false, "MultipleBookedIntervals_Overlap_ShouldReturnFalse");
    }

    public void Test_BookedIntervalsNull_ShouldReturnTrue()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var result = validator.CanBook(requested, null, 30);
        AssertHelper.Assert(result == true, "BookedIntervalsNull_ShouldReturnTrue");
    }

    public void Test_RequestedNull_ShouldThrowException()
    {
        var validator = new BookingValidator();
        
        AssertHelper.AssertThrows<ArgumentNullException>(
            () => validator.CanBook(null, new List<TimeInterval>(), 30),
            "RequestedNull_ShouldThrowException"
        );
    }

    public void Test_NegativeMinGap_ShouldThrowException()
    {
        var validator = new BookingValidator();
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        AssertHelper.AssertThrows<ArgumentException>(
            () => validator.CanBook(requested, new List<TimeInterval>(), -5),
            "NegativeMinGap_ShouldThrowException"
        );
    }
}