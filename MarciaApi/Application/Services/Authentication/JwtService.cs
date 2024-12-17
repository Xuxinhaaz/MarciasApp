using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.Errors.AuthErrors;
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

    public async Task<ErrorOr<string>> Generate(UserModel model)
    {
        var hanlder = new JwtSecurityTokenHandler();

        var claims = new List<Claim>();
        
        claims.Add(new Claim(ClaimTypes.Email, model.Email));
        
        foreach (var role in model.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role));
        }

        var claimsIndetity = new ClaimsIdentity(claims);

        var experies = DateTime.UtcNow.AddDays(30);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claimsIndetity,
            Expires = experies,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"])),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var createdToken = hanlder.CreateToken(tokenDescriptor);

        var token = hanlder.WriteToken(createdToken);

        if (token is null)
            return JwtErrors.TokenIsNull;

        return token;
    }

    public async Task<ErrorOr<string>> Validate(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)),
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true
        };

        var handler = new JwtSecurityTokenHandler();
        
        var validationResult = await handler.ValidateTokenAsync(token, tokenValidationParameters);
        if (!validationResult.IsValid)
            return JwtErrors.NotSignedUp;
        
        return token;
    }

    public async Task<ErrorOr<string>> ValidateMangager(string token)
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

        
            var validationResult = await handler.ValidateTokenAsync(token, tokenValidationParameters);

            if (!validationResult.IsValid)
                return JwtErrors.NotSignedUp;
            
            var claimsIdentity = validationResult.ClaimsIdentity;
            if (claimsIdentity.HasClaim(ClaimTypes.Role, "Manager")) return token;

            return JwtErrors.UserIsNotAllowed;
    }
}