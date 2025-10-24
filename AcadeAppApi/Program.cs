using Microsoft.EntityFrameworkCore;
using AcadeAppApi.Data;

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

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();

app.MapControllers();  // map API controllers

app.Run();
