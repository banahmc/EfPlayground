namespace Model.Services.SystemClock;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
