using Authentication.Data;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<FFContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.WebHost.ConfigureKestrel(options =>
//{
//	options.ListenAnyIP(4642); // to listen for incoming http connection on port 5001
//});


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();
app.MapControllers();



app.Run();



//using var db = new FFContextDb();
//var user = db.Users
//    .Select(x => x);

//foreach (User u in user)
//{
//    db.Users.Remove(u);
//    db.SaveChanges();
//}