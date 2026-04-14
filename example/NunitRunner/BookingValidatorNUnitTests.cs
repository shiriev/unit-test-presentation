using BookingLogic;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NunitRunner;

[TestFixture]
public class BookingValidatorNUnitTests
{
    private BookingValidator _validator;
    
    [SetUp]
    public void Setup()
    {
        _validator = new BookingValidator();
    }

    [Test]
    public void CanBook_NoBookedIntervals_ReturnsTrue()
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );

        // Act
        var result = _validator.CanBook(requested, new List<TimeInterval>(), 30);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void CanBook_NoOverlap_ReturnsTrue()
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 14, 0, 0),
            new DateTime(2024, 1, 1, 15, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 10, 0, 0),
                new DateTime(2024, 1, 1, 11, 0, 0)
            )
        };

        // Act
        var result = _validator.CanBook(requested, bookedIntervals, 30);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void CanBook_ExactOverlap_ReturnsFalse()
    {
        // Arrange
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

        // Act
        var result = _validator.CanBook(requested, bookedIntervals, 30);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void CanBook_PartialOverlap_ReturnsFalse()
    {
        // Arrange
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

        // Act
        var result = _validator.CanBook(requested, bookedIntervals, 30);

        // Assert
        Assert.IsFalse(result);
    }

    [TestCase(60, true)]  // Зазор 60 минут, минимальный 30 - должно быть OK
    [TestCase(30, true)]  // Зазор точно 30 минут - должно быть OK
    [TestCase(15, false)] // Зазор 15 минут, минимальный 30 - НЕ OK
    public void CanBook_RespectsMinGapAtStart_ReturnsExpectedResult(int gapMinutes, bool expectedResult)
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedEndTime = new DateTime(2024, 1, 1, 9, 0, 0).AddMinutes(gapMinutes);
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                new DateTime(2024, 1, 1, 8, 0, 0),
                bookedEndTime
            )
        };

        // Act
        var result = _validator.CanBook(requested, bookedIntervals, 30);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    [TestCase(60, true)]  // Зазор 60 минут, минимальный 30 - должно быть OK
    [TestCase(30, true)]  // Зазор точно 30 минут - должно быть OK
    [TestCase(15, false)] // Зазор 15 минут, минимальный 30 - НЕ OK
    public void CanBook_RespectsMinGapAtEnd_ReturnsExpectedResult(int gapMinutes, bool expectedResult)
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );
        
        var bookedStartTime = new DateTime(2024, 1, 1, 11, 0, 0).AddMinutes(gapMinutes);
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(
                bookedStartTime,
                new DateTime(2024, 1, 1, 13, 0, 0)
            )
        };

        // Act
        var result = _validator.CanBook(requested, bookedIntervals, 30);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void CanBook_WithMultipleBookedIntervalsAndGaps_ReturnsCorrectResult()
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 12, 0, 0),
            new DateTime(2024, 1, 1, 13, 0, 0)
        );
        
        var bookedIntervals = new List<TimeInterval>
        {
            new TimeInterval(new DateTime(2024, 1, 1, 9, 0, 0), new DateTime(2024, 1, 1, 10, 0, 0)),
            new TimeInterval(new DateTime(2024, 1, 1, 10, 45, 0), new DateTime(2024, 1, 1, 11, 15, 0)),
            new TimeInterval(new DateTime(2024, 1, 1, 14, 0, 0), new DateTime(2024, 1, 1, 15, 0, 0))
        };

        // Act
        var result = _validator.CanBook(requested, bookedIntervals, 30);

        // Assert
        // Зазор между 11:15 и 12:00 = 45 минут, что > 30 минут, поэтому должно быть OK
        Assert.IsTrue(result);
    }

    [Test]
    public void CanBook_BookedIntervalsNull_ReturnsTrue()
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );

        // Act
        var result = _validator.CanBook(requested, null, 30);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void CanBook_RequestedNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => _validator.CanBook(null, new List<TimeInterval>(), 30)
        );
    }

    [Test]
    public void CanBook_NegativeMinGap_ThrowsArgumentException()
    {
        // Arrange
        var requested = new TimeInterval(
            new DateTime(2024, 1, 1, 10, 0, 0),
            new DateTime(2024, 1, 1, 11, 0, 0)
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _validator.CanBook(requested, new List<TimeInterval>(), -5)
        );
    }

    [Test]
    public void TimeInterval_StartGreaterThanEnd_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new TimeInterval(
                new DateTime(2024, 1, 1, 11, 0, 0),
                new DateTime(2024, 1, 1, 10, 0, 0)
            )
        );
    }

    [Test]
    public void TimeInterval_StartEqualsEnd_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => new TimeInterval(
                new DateTime(2024, 1, 1, 10, 0, 0),
                new DateTime(2024, 1, 1, 10, 0, 0)
            )
        );
    }
}
