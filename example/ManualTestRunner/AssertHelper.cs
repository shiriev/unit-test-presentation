namespace ManualTestRunner;

public static class AssertHelper
{
    public static void Assert(bool condition, string testName)
    {
        if (condition)
        {
            Console.WriteLine($"✓ PASS: {testName}");
            // _passedTests++;
        }
        else
        {
            Console.WriteLine($"✗ FAIL: {testName}");
            // _failedTests++;
        }
    }

    public static void AssertThrows<TException>(Action action, string testName) where TException : Exception
    {
        try
        {
            action();
            Console.WriteLine($"✗ FAIL: {testName} (Expected {typeof(TException).Name} but no exception was thrown)");
            // _failedTests++;
        }
        catch (TException)
        {
            Console.WriteLine($"✓ PASS: {testName}");
            // _passedTests++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ FAIL: {testName} (Expected {typeof(TException).Name} but got {ex.GetType().Name})");
            // _failedTests++;
        }
    }
}