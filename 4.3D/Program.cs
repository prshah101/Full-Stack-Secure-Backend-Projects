using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using robot_controller_api.Persistence;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

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
