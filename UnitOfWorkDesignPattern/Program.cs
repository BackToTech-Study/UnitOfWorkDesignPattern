using Microsoft.EntityFrameworkCore;
using UnitOfWorkDesignPattern.DataStorage;
using UnitOfWorkDesignPattern.Models.MapperProfiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(OrderMapperProfile), typeof(ProductMapperProfile));

var connectionString = builder.Configuration.GetConnectionString("ApplicationDatabase");
builder.Services.AddDbContext<ApplicationDataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    );
builder.Services.AddScoped<UnitOfWork>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}