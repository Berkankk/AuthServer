using Microsoft.AspNetCore.Authorization;
using MiniAPP1.API.ClaimsRequirements;
using SharedLibrary.Configurations;
using SharedLibrary.Extensions;
using static MiniAPP1.API.ClaimsRequirements.BirthDayRequirement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.Configure<CustomTokenOptions>(configuration.GetSection("TokenOption"));

var tokenOptions = configuration.GetSection("TokenOption").Get<CustomTokenOptions>();

builder.Services.AddCustomTokenAuth(tokenOptions);
builder.Services.AddSingleton<IAuthorizationHandler, BirthDayRequirementHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AnkaraPolicy", policy =>   //Burasý hem admin hem de city rolünde olanlar için geçerli
    {
        policy.RequireClaim("City", "Ankara");
    });

    options.AddPolicy("AgePolicy", policy =>
    {
        policy.Requirements.Add(new BirthDayRequirement(18));
    });

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
