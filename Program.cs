using ANPCentral.Data;
using ANPCentral.Models;
using ANPCentral.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<UserDataContext>();

builder.Services.AddScoped<TokenService>();

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseAuthentication();

app.MapControllers();

app.Run();
