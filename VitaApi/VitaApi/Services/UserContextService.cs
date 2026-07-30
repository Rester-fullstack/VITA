using System.Security.Claims;
using VitaApi.Interfaces;

namespace VitaApi.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var value =
                _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirst(
                        ClaimTypes.NameIdentifier
                    )?.Value;

            if (
                int.TryParse(
                    value,
                    out var id
                )
            )
            {
                return id;
            }

            return null;
        }
    }

    public string? UserName =>
        _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(
                ClaimTypes.Name
            )?.Value;

    public string? Role =>
        _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(
                ClaimTypes.Role
            )?.Value;

    public bool IsAdmin =>
        Role == "Admin";

    public bool IsMedico =>
        Role == "Medico";
}