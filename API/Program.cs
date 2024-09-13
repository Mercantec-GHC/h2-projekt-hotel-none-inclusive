using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration Configuration = builder.Configuration;
string connectionString = Configuration.GetConnectionString("DefaultConnection");

// Add database context and mapping services
builder.Services.AddDbContext<DBContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<UserMapping>(); 
builder.Services.AddScoped<RoomMapping>();
builder.Services.AddScoped<BookingMapping>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
