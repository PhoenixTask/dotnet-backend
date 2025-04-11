namespace SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
