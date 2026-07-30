namespace VitaApi.Interfaces;

public interface IUserContextService
{
    int? UserId { get; }

    string? UserName { get; }

    string? Role { get; }

    bool IsAdmin { get; }

    bool IsMedico { get; }
}