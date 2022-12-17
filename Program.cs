using Authentication.Data;
using Authentication.Models;
using Authentication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<FFContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(options => options.WithOrigins(new[] { "http://localhost:3000", "http://localhost:8080", "http://localhost:4200" }).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

app.UseAuthorization();
app.MapControllers();


if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    app.Run("http://localhost:8000");
}

//using var db = new FFContextDb();
//var user = db.Users
//    .Select(x => x);

//foreach (User u in user)
//{
//    db.Users.Remove(u);
//    db.SaveChanges();
//}