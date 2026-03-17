using Love.Domain.Enums;

namespace Love.Domain.Models;

public class LoginResult
{
    public required bool IsLogged { get; init; }
    public Role?  Role { get; init; }
}