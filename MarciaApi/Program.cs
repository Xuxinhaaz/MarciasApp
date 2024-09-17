using System.Text;
using MarciaApi.Application;
using MarciaApi.Domain.Data.Cloud;
using MarciaApi.Infrastructure;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Infrastructure.Data.Cloud;
using MarciaApi.Infrastructure.Data.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(x => 
    x.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddTransient<DataSeeder>();
builder.Services.AddScoped<ICloudflare, Cloudflare>(x => new Cloudflare(builder.Configuration));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddLogging(x =>
{
    x.AddConsole(); 
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seedata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service.Seed();
    }
}

app.UseRouting();
app.MapControllers();

app.Run();