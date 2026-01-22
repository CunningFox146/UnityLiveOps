using CunningFox.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/active-liveops", LiveOpGenerator.GetCalendar)
    .WithName("GetActiveLiveOps");

app.Run();

internal static class LiveOpGenerator
{
    private static readonly List<LiveOpDto> Settings =
    [
        new(Guid.NewGuid(), DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddSeconds(10), "Test", 0),
        new(Guid.NewGuid(), DateTime.UtcNow.AddHours(-5), DateTime.UtcNow.AddMinutes(5), "Test", 3),
        new(Guid.NewGuid(), DateTime.UtcNow.AddHours(-2), DateTime.UtcNow.AddHours(1), "Test", 5),
        new(Guid.NewGuid(), DateTime.UtcNow.AddHours(-2), DateTime.UtcNow.AddMinutes(1), "Test", 7),
    ];

    public static LiveOpCalendarDto GetCalendar() => new(Guid.NewGuid(), DateTime.UtcNow.Ticks, Settings);
}
