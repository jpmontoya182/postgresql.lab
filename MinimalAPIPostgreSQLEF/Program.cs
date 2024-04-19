using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgreSQLEF.Data;
using MinimalAPIPostgreSQLEF.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<OfficeDBContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/employees", async (Employee e, OfficeDBContext dbContext) =>
{
    dbContext.Employees.Add(e);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/employee/{e.Id}", e);
});

app.MapGet("/employee/{id:int}", async (int id, OfficeDBContext dbContext) =>
{
    return await dbContext.Employees.FindAsync(id)
    is Employee e
    ? Results.Ok(e)
    : Results.NotFound();
});

app.Run();