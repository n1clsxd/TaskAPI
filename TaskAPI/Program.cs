using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TasksDB"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => "Olá Mundo");

app.MapGet("/tasks", async (AppDbContext db) => await db.Tasks.ToListAsync());

app.MapGet("/tasks/completed", async (AppDbContext db) => await db.Tasks.Where(task => task.isCompleted).ToListAsync());

app.MapGet("/tasks/{id}", async (int id, AppDbContext db) => await db.Tasks.FindAsync(id) is Task task ? Results.Ok(task) : Results.NotFound());

app.MapPost("/tasks", async (Task task, AppDbContext db) =>{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id}", async (int id, Task inputTask, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    task.name = inputTask.name;
    task.isCompleted = inputTask.isCompleted;

    await db.SaveChangesAsync();
    return Results.Ok(inputTask);
});

app.MapDelete("/tasks/{id}", async(int id, AppDbContext db) =>
{
    if(await db.Tasks.FindAsync(id) is Task task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.Ok(task);

    }

    return Results.NotFound();
});

app.Run();

[PrimaryKey(nameof(Id))]
class Task
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? name { get; set; }

    [Required]
    public bool isCompleted { get; set; }
}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options) { }

    public DbSet<Task> Tasks => Set<Task>();
}