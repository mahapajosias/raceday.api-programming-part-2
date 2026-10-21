
using Microsoft.EntityFrameworkCore;
using RaceDay.Server.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("RaceDayConnection");

builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/health/database", async (RaceDayDbContext db) =>
{
    try
    {
        bool connected = await db.Database.CanConnectAsync();

        return connected
            ? Results.Ok(new
            {
                status = "Success",
                message = "Connected to the RaceDay MySQL database."
            })
            : Results.Problem(
                title: "Database connection failed");
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Database connection failed",
            detail: ex.Message);
    }
});

app.MapControllers();

app.Run();
