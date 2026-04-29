using Microsoft.EntityFrameworkCore;
using PakDzal_Games_API.Data;
using PakDzal_Games_API.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Database=pakdzal_games;Username=postgres;Password=111";

builder.Services.AddDbContext<GameClubDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine("База данных готова!");

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

app.Run();