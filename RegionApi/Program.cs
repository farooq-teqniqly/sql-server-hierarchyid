using Microsoft.EntityFrameworkCore;
using RegionApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HidsampleContext>(opts =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("RegionDb")
        ?? throw new InvalidOperationException("Database connection string was not specified.");

    opts.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
