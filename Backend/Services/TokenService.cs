using Backend.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration; // Variavel de acesso as configurações
        }
        public string Generate(UsuarioResponseDto dto)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var key = Encoding.UTF8.GetBytes(jwtKey);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            // Criar uma instancia do JwtSecurityTokenHandler   
            var handler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
               {
                    new Claim(ClaimTypes.NameIdentifier,dto.Id.ToString()),   // Payload do token
                    new Claim(ClaimTypes.Email, dto.Email)
                    
                }),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"]
            };

            // Gerar um token
            var token = handler.CreateToken(tokenDescriptor);
            
            // Gera uma string do token
             return handler.WriteToken(token);

        }
    }
}
