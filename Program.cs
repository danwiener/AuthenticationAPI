using Authentication.Data;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();


if (builder.Environment.IsDevelopment())
{
	builder.Services.AddDbContext<FFContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else if (builder.Environment.IsProduction())
{
		builder.Services.AddDbContext<FFContextDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));
}



var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();
app.MapControllers();



app.Run();

