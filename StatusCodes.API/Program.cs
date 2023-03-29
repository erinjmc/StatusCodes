using Microsoft.EntityFrameworkCore;
using StatusCodes.API.DbContext;
using StatusCodes.API.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; });

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Repo 
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
//DbContext
builder.Services.AddDbContext<StatusCodesDbContext>(options => {
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:StatusCodesDbContextConnection"]);
});

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options => 
    { 
        options.TokenValidationParameters = new()
        { 
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        }; 
    }
);

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

DbInit.Seed(app);

app.Run();
