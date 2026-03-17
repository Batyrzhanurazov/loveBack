using Love.Domain.Enums;

namespace Love.Domain.Models;

public sealed record User
{
    public required string Login { get; init; }
    public required string Password { get; init; }
    public required Role Role { get; init; }
}