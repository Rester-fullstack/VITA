using VitaApi.DTOs.Auth;
using VitaApi.Helpers;
using VitaApi.Interfaces;
using VitaApi.Models;
using VitaApi.Responses;

namespace VitaApi.Services;

public class AuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IMedicoRepository _medicoRepository;
    

    public AuthService(
     IUserRepository repository,
     IConfiguration configuration,
     IRefreshTokenRepository refreshRepository,
     IMedicoRepository medicoRepository
    )
    {
        _repository = repository;
        _configuration = configuration;
        _refreshRepository = refreshRepository;
        _medicoRepository = medicoRepository;
    }

  
    public async Task<ApiResponse<object>> Register(RegisterDto dto)
    {
        var exists = await _repository.GetByEmailAsync(dto.Email);

        if (exists != null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Email já cadastrado"
            };
        }

        var user = new User
        {
            Nome = dto.Nome,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Usuário criado com sucesso",
            Data = new
            {
                id = user.Id,
                nome = user.Nome,
                email = user.Email,
                role = user.Role
            }
        };
    }

    
    public async Task<ApiResponse<object>> Login(LoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Usuário não encontrado"
            };
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!passwordValid)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Senha inválida"
            };
        }

        var token = JwtHelper.GenerateToken(user, _configuration);
        var refreshToken = RefreshTokenHelper.Generate();

        var refreshDays = int.Parse(
            _configuration["Jwt:RefreshTokenExpirationDays"]!
        );

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
            IsRevoked = false
        };

        await _refreshRepository.AddAsync(refreshTokenEntity);
        await _refreshRepository.SaveChangesAsync();

        Medico? medico = null;

        if (user.Role == "Medico")
        {
            medico = await _medicoRepository.GetByUserIdAsync(user.Id);
        }

        Console.WriteLine($"USER ID: {user.Id}");

        if (medico == null)
        {
            Console.WriteLine("MEDICO = NULL");
        }
        else
        {
            Console.WriteLine($"CRM = {medico.CRM}");
            Console.WriteLine($"ESPECIALIDADE = {medico.Especialidade?.Nome}");
        }

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Login realizado",

            Data = new
            {
                token,
                refreshToken,

                user.Id,
                user.Nome,
                user.Email,
                user.Role,

                crm = medico?.CRM,
                especialidade = medico?.Especialidade?.Nome
            }
        };
    }

    
    public async Task<ApiResponse<object>> RefreshToken(RefreshTokenRequestDto dto)
    {
        var refreshToken = await _refreshRepository.GetByTokenAsync(dto.RefreshToken);

        if (
            refreshToken == null ||
            refreshToken.IsRevoked ||
            refreshToken.ExpiresAt < DateTime.UtcNow
        )
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Refresh token inválido"
            };
        }

        var user = await _repository.GetByIdAsync(refreshToken.UserId);

        if (user == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Usuário não encontrado"
            };
        }

        var newAccessToken = JwtHelper.GenerateToken(user, _configuration);
        var newRefreshToken = RefreshTokenHelper.Generate();

        var refreshDays = int.Parse(
            _configuration["Jwt:RefreshTokenExpirationDays"]!
        );

        refreshToken.IsRevoked = true;

        var refresh = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
            IsRevoked = false
        };

        await _refreshRepository.AddAsync(refresh);
        await _refreshRepository.SaveChangesAsync();

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Token renovado",

            Data = new
            {
                token = newAccessToken,
                refreshToken = newRefreshToken
            }
        };
    }

    
    public async Task<ApiResponse<object>> Logout(string token)
    {
        var refreshToken = await _refreshRepository.GetByTokenAsync(token);

        if (refreshToken == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Token inválido"
            };
        }

        if (refreshToken.IsRevoked)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Token já revogado"
            };
        }

        refreshToken.IsRevoked = true;

        await _refreshRepository.SaveChangesAsync();

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Logout realizado com sucesso"
        };
    }
}