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
    return Results.Created($"/employees/{e.Id}", e);
});

app.MapGet("/employees/{id:int}", async (int id, OfficeDBContext dbContext) =>
{
    return await dbContext.Employees.FindAsync(id)
    is Employee e
    ? Results.Ok(e)
    : Results.NotFound();
});

app.MapGet("/employees", async (OfficeDBContext dbContext) => await dbContext.Employees.ToListAsync());

app.MapPut("/employees/{id:int}", async (int id, Employee e, OfficeDBContext dbContext) =>
{
    if (e.Id != id)
        return Results.BadRequest();

    var employee = await dbContext.Employees.FindAsync(e.Id);

    if (employee is null) return Results.BadRequest();

    employee.FirstName = e.FirstName;
    employee.LastName = e.LastName;
    employee.Branch = e.Branch;
    employee.Age    = e.Age;

   await dbContext.SaveChangesAsync();

    return Results.Ok(employee);
});

app.MapDelete("/employees/{id:int}", async (int id, OfficeDBContext dBContext) =>
{
    var employe = await dBContext.Employees.FindAsync(id);

    if (employe is null) return Results.NotFound();
    
    dBContext.Employees.Remove(employe);
    await dBContext.SaveChangesAsync();

    return Results.NoContent();
});


app.Run();