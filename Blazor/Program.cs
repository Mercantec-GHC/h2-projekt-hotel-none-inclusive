using Blazor.Components;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adds Blazor server-side components with interactive server-side rendering.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

// Register the DatabaseServices class as a scoped service
builder.Services.AddScoped<DatabaseServices>();

// Register the DBContext with SQL Server for Entity Framework
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Sets up error handling for non-development environments
if (!app.Environment.IsDevelopment())
{
    // Use a generic error handler and HTTP Strict Transport Security (HSTS) in production
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Common middlewares for handling HTTPS redirection, serving static files, and enabling anti-forgery
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Map Razor components for server-side rendering with interactive components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
