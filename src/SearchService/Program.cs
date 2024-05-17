using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

try
{
    await DbInitializer.InitializeDatabase(app);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.Run();
