using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServicioUsuarios.Dominio.Modelos;
using ServicioUsuarios.Dominio.Puertos.PuertoSalida;

namespace ServicioUsuarios.Infraestructura.Ayudadores
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerarToken(Usuario usuario)
        {
            var key = config["Jwt:Key"];
            var issuer = config["Jwt:Issuer"];
            var audience = config["Jwt:Audience"];
            var expiration = config.GetValue<int>("Jwt:ExpirationMinutes");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}