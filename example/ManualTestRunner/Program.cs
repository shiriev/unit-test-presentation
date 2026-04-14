using ManualTestRunner;

Console.WriteLine("=== Unit Tests Learning Example ===\n");

// Запуск простых тестов без тест-ранера
var simpleTests = new SimpleTests();
simpleTests.RunAllTests();

Console.ReadLine();