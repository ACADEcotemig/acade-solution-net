using Microsoft.EntityFrameworkCore;
using AcadeAppApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AcadeAppApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();                       // enable controllers
builder.Services.AddDbContext<AppDbContext>(options =>   // EF Core DbContext
    options.UseSqlite("Data Source=acadeapp.db"));
builder.Services.AddCors(options =>                      // allow Blazor client to call API
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT setup - in production move secret to configuration or Key Vault
var jwtKey = builder.Configuration["Jwt:Key"] ?? "replace_this_with_a_long_secret_change_this";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddSingleton<ITokenService>(new JwtTokenService(keyBytes));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();  // map API controllers

app.Run();
