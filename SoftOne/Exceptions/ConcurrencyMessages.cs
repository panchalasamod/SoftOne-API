namespace SoftOne.Exceptions;

/// <summary>
/// Shared concurrency conflict messages for optimistic RowVersion checks.
/// </summary>
public static class ConcurrencyMessages
{
    public const string AlreadyUpdated = "Task has already updated from another user";
}
