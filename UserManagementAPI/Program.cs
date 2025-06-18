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

// Boilerplate for User Management API
app.MapGet("/", () => "Welcome to the User Management API for TechHive Solutions!");

// In-memory user store for demonstration
var users = new List<User>
{
    new User { Id = 1, Name = "Alice", Department = "HR", Email = "alice@techhive.com" },
    new User { Id = 2, Name = "Bob", Department = "IT", Email = "bob@techhive.com" }
};

// GET: Retrieve all users
app.MapGet("/users", () => users);

// GET: Retrieve a user by ID
app.MapGet("/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// POST: Add a new user
app.MapPost("/users", (User user) =>
{
    user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

// PUT: Update an existing user
app.MapPut("/users/{id:int}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    user.Name = updatedUser.Name;
    user.Department = updatedUser.Department;
    user.Email = updatedUser.Email;
    return Results.Ok(user);
});

// DELETE: Remove a user by ID
app.MapDelete("/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    users.Remove(user);
    return Results.NoContent();
});

app.Run();

// User model (moved to bottom to avoid top-level statement error)
record User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
