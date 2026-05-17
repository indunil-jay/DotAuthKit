using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class JwtProvider(
    IOptions<JwtOptions> options,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager)
    : IJwtProvider
{
    private const string PermissionClaimType = "permissions";
    private readonly JwtOptions _jwtOptions = options.Value;

    public async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        ApplicationUser? appUser = await userManager.FindByIdAsync(user.Id.ToString());

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (appUser is not null)
        {
            IList<string> roles = await userManager.GetRolesAsync(appUser);

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var permissionsAdded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string roleName in roles)
            {
                ApplicationRole? roleEntity = await roleManager.FindByNameAsync(roleName);
                if (roleEntity is null)
                {
                    continue;
                }

                IList<Claim> roleClaims = await roleManager.GetClaimsAsync(roleEntity);

                foreach (Claim claim in roleClaims.Where(c => c.Type == PermissionClaimType))
                {
                    if (permissionsAdded.Add(claim.Value))
                    {
                        claims.Add(new Claim(PermissionClaimType, claim.Value));
                    }
                }
            }
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: null,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
