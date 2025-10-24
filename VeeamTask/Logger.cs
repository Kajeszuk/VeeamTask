class Logger
{
    private readonly string loggerPath;
    public Logger(string loggerPath) 
    {
        this.loggerPath = loggerPath;
        File.WriteAllText(loggerPath, string.Empty);
    }

    public void LogMessage(string message)
    {
        Console.WriteLine(message);
        File.AppendAllText(loggerPath, message + Environment.NewLine);
    }
}
