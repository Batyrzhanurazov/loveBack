namespace Love.Domain.Models;

public sealed record LoveMessage
{
    public required string Message { get; init; }
    public required bool IsUsed { get; init; }
}