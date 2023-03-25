using Microsoft.EntityFrameworkCore;
using StatusCodes.API.Models;
using StatusCodes.API.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repo for status code
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

builder.Services.AddDbContext<StatusCodesDbContext>(options => {
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:StatusCodesDbContextConnection"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
