using AuthServer.API.Validation;
using AuthServer.Core.Configuration;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.DataAccess.Context;
using AuthServer.DataAccess.Repositories;
using AuthServer.DataAccess.UnitOf;
using AuthServer.Services.Authentication;
using AuthServer.Services.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System.Text;
using SharedLibrary.Validation;
using SharedLibrary.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();  //FluentVAlidationu geçtik burada 
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<CreateUserValidator>();

builder.Services.UseCustomValidationResponse();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection (DI) registration
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Identity Configuration
builder.Services.AddIdentity<UserApp, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>();

// Configuration from appsettings.json
var configuration = builder.Configuration;
builder.Services.Configure<CustomTokenOptions>(configuration.GetSection("TokenOption"));
builder.Services.Configure<List<Client>>(configuration.GetSection("Clients"));

var tokenOptions = configuration.GetSection("TokenOption").Get<CustomTokenOptions>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    opts.RequireHttpsMetadata = false; // Dev ortamýnda SSL zorunlu deðil
    opts.SaveToken = true;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,  //
        ValidateLifetime = true,   //Ömrü yaþamý son kullanma tarihi
        ValidateIssuerSigningKey = true,  //imzasý 
        ValidIssuer = tokenOptions.Issuer, // appsettings.json'dan gelen deðer
        ValidAudiences = tokenOptions.Audience, // appsettings.json'dan gelen deðer
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ClockSkew = TimeSpan.Zero // Token süresi kontrolünde sapma olmaz(bizde yanlýþ olmaz) test etmek için 0 tuttuk yoksa 0 olmaz 
    };
});


var app = builder.Build();




// HTTP request pipeline configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCustomException(); //Bunu shared dan geçtik
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Authentication middleware’i ekledim
app.UseAuthorization();

app.MapControllers();

app.Run();
