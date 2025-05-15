
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using System;
using System.Net;
using System.Text;
using WFH.Api.Dependencies;
using WFH.infrastructure.Depencencies;
using WorkFromHome.Domain.models;
using WorkFromHome.Infrastructure;
using static System.Net.WebRequestMethods;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ServiceConfiguration(builder.Configuration);

builder.Services.Application();


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
 
})
        .AddEntityFrameworkStores<WorkFromHomeDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        c.OAuthClientId(builder.Configuration["Okta:ClientId"]);
        c.OAuthUsePkce(); 
    });

}

app.UseHttpsRedirection();
app.UseCors();
//app.UseCors("AllowLocalhost4200");

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();




app.MapControllers();

app.Run();
