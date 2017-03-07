namespace Announcer.Interfaces
{
    public interface ILogWriter
    {
        void WriteLogFile(string message, Logger.Severity severity);
    }
}
