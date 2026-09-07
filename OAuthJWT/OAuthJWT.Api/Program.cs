using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OAuthJWT.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var key = Encoding.ASCII.GetBytes("TuSuperClaveSecretaMuyLargaParaJWT123456789!");

            app.MapPost("/api/auth/login", (LoginModel model) =>
            {
                // Validación con datos quemados
                if (model.Username == "admin" && model.Password == "1234")
                {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin")
            }),
                        Expires = DateTime.UtcNow.AddHours(2),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    return Results.Ok(new { Token = tokenHandler.WriteToken(token) });
                }
                return Results.Unauthorized();
            });

            app.Run();
        }
    }
}

public class LoginModel { public string Username { get; set; } public string Password { get; set; } }
