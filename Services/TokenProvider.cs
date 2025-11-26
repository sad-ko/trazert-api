using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Trazert_API.Database.Models;

namespace Trazert_API.Services;

internal sealed class TokenProvider(IConfiguration configuration)
{
    public string Create(Usuario user)
    {
        //var categoria = await context.Categoria.FirstAsync(cat => cat.Id == user.Categoria_Id);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Nombre),
                //new Claim(ClaimTypes.Role, categoria.Descripcion),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}