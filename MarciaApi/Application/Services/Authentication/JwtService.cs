using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.IdentityModel.Tokens;

namespace MarciaApi.Application.Services.Authentication;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> Generate(UserViewModel model)
    {
        var hanlder = new JwtSecurityTokenHandler();

        var claims = new ClaimsIdentity(new []
        {
            new Claim(ClaimTypes.Email, model.Email)
        });

        var experies = DateTime.UtcNow.AddDays(2);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claims,
            Expires = experies,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"])),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var createdToken = hanlder.CreateToken(tokenDescriptor);

        var token = hanlder.WriteToken(createdToken);

        return token;
    }

    public async Task<bool> Validate(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true
        };

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var validationResult = await handler.ValidateTokenAsync(token, tokenValidationParameters);
            return validationResult.IsValid;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex.ToString());
            return false;
        }
    }
}