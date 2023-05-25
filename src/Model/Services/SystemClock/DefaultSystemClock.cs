namespace Model.Services.SystemClock;

public sealed class DefaultSystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
