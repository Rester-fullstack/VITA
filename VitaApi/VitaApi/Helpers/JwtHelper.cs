using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using VitaApi.Models;

namespace VitaApi.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(
        User user,
        IConfiguration configuration
    )
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()
            ),

            new Claim(
                ClaimTypes.Name,
                user.Nome
            ),

            new Claim(
                ClaimTypes.Email,
                user.Email
            ),

            new Claim(
                ClaimTypes.Role,
                user.Role
            )
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["Jwt:Key"]!
                )
            );

        var creds =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

        var expirationMinutes =
            int.Parse(
                configuration[
                    "Jwt:AccessTokenExpirationMinutes"
                ]!
            );

        var token =
            new JwtSecurityToken(
                issuer:
                    configuration["Jwt:Issuer"],

                audience:
                    configuration["Jwt:Audience"],

                claims: claims,

                expires:
                    DateTime.UtcNow.AddMinutes(
                        expirationMinutes
                    ),

                signingCredentials: creds
            );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}