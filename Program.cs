﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(@"
╔═══════════════════════════════════════════╗
║            API LAUNCH IN PROGRESS         ║
╚═══════════════════════════════════════════╝");
Console.ResetColor();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(@"
📦 Services Configuration:
└── 🏗️  Builder created");
Console.ResetColor();

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("    └── 🎮 Controllers added");
Console.ResetColor();

var app = builder.Build();
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("\n🔧 Application Configuration:");
Console.WriteLine("└── 🏭 Application built");
Console.ResetColor();

// Test database connection and seed data
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.OpenConnection();
        context.Database.CloseConnection();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("    └── 🔌 Database connection successful");
        Console.ResetColor();

        // Seed the database
        DatabaseInitializer.Seed(scope.ServiceProvider);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("    └── 🌱 Database seeded successfully");
        Console.ResetColor();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"    └── ❌ Database connection or seeding failed: {ex.Message}");
    Console.ResetColor();
    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("    └── 🐛 Development mode enabled");
    Console.ResetColor();
}

app.UseHttpsRedirection();
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("    └── 🔒 HTTPS redirection enabled");
Console.ResetColor();

app.UseRouting();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("    └── 🛣️  Routes configured");
Console.ResetColor();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("    └── 🎯 Endpoints mapped");
Console.ResetColor();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(@"
╔═══════════════════════════════════════════╗
║         API STARTED SUCCESSFULLY          ║
╚═══════════════════════════════════════════╝");
Console.ResetColor();

app.Run();
