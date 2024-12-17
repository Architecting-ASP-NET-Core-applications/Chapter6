namespace Chapter6CustomMiddleware.Models;

/// <summary>
/// Example for page 19
/// </summary>
//public class CustomLogger
//{
//    public void Log(string message)
//        => Console.WriteLine($"Custom Logger: {message}");
//}

/// <summary>
/// Example for page 20
/// </summary>
public class CustomLogger
{
    public CustomLogger(LoggerConfig config)
    {
        // Imagine some complex initialization here using LoggerConfig
    }
    public void Log(string message)
    {
        Console.WriteLine($"CustomLogger: {message}");
    }
}

