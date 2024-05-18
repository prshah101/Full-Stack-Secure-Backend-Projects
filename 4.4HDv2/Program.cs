using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using robot_controller_api.Persistence;
using robot_controller_api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<RobotContext>(options =>
    options.UseNpgsql("Host=localhost;Database=sit331;Username=postgres;Password=password"));

// 4.1P
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
//builder.Services.AddScoped<IMapDataAccess, MapADO>();

// 4.2C
// builder.Services.AddScoped<IRobotCommandDataAccess,
// RobotCommandRepository>();
// builder.Services.AddScoped<IMapDataAccess, MapRepository>();

// 4.3D:
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
