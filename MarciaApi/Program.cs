using MarciaApi.Application;
using MarciaApi.Infrastructure;
using MarciaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(x => 
    x.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();



app.UseRouting();
app.MapControllers();

app.Run();