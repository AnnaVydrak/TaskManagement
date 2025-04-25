namespace TaskManagement.Core.Common.Providers;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}