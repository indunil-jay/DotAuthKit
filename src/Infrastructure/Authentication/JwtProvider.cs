using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }


    public string GenerateToken(User user)
    {
        var claims = new Claim[] { 
         new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
         new(JwtRegisteredClaimNames.Email, user.Email),
        };

        var signingCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                                    SecurityAlgorithms.HmacSha256);

        var token  = new JwtSecurityToken(
                            issuer: _jwtOptions.Issuer,
                            audience: _jwtOptions.Audience, 
                            claims: claims,
                            notBefore: null,
                            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes), 
                            signingCredentials
            );


        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
