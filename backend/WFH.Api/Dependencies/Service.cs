using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using System.Net;
using System.Text;
using System.Text.Json;
using WFH.Api.Controllers;
using WorkFromHome.Infrastructure;

namespace WFH.Api.Dependencies
{
    public static class Service
    {
        public static void ServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<WorkFromHomeDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("dbconn")),
                ServiceLifetime.Scoped
            );
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
               .AddJwtBearer("Okta", options =>
               {
                   options.Authority = configuration["Okta:Authority"];
                   options.Audience = configuration["Okta:Audience"];

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = false,
                       ValidateIssuerSigningKey = true,
                       ClockSkew=TimeSpan.Zero,


                       IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                       {
                           var client = new HttpClient();
                           var keySet = client.GetStringAsync(configuration.GetSection("Okta:keySet").Value).Result;
                           var keys = new JsonWebKeySet(keySet);
                           return keys.GetSigningKeys();
                       }
                   };
                       options.Events = new JwtBearerEvents
                       {
                           OnAuthenticationFailed = context =>
                           {
                               if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                               {
                                   context.Response.Headers.Add("Token-Expired", "true");
                               }
                               return Task.CompletedTask;
                           }
                       };
               }
                )
               .AddJwtBearer("Custom", options =>
               {

                   options.SaveToken = true;
                
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                       ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Jwt:SecretKey").Value!)),
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidateLifetime = true,
                       ClockSkew=TimeSpan.Zero,
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                              
                               context.Response.Headers.Add("Token-Expired", "true");
                               context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                               context.Response.ContentType = "application/json";

                               var result = JsonSerializer.Serialize(new { message = "Token expired" });
                               return context.Response.WriteAsync(result);
                           }
                           return Task.CompletedTask;
                       }
                   };

               });

            //        services.AddSwaggerGen(c =>
            //        {
            //            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //            {
            //                Type = SecuritySchemeType.OAuth2,
            //                Flows = new OpenApiOAuthFlows
            //                {
            //                    AuthorizationCode = new OpenApiOAuthFlow
            //                    {
            //                        AuthorizationUrl =new Uri(configuration["Okta:AuthorizationUrl"]!),
            //                        TokenUrl = new Uri(configuration["Okta:TokenUrl"]!),
            //                        Scopes = new Dictionary<string, string>
            //            {
            //                { "openid", "Read access to protected API" }
            //            }
            //                    }
            //                }
            //            });

            //            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "oauth2",

            //            },In = ParameterLocation.Header
            //        },
            //        new[] { "openid" }
            //    }
            //});
            //        });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerEmployee", policy =>
                {
                    policy.RequireRole("Manager", "Employee");

                });
                options.AddPolicy("ManagerOnly", policy =>
                {
                    policy.RequireRole("Manager");
                });
                options.AddPolicy("EmployeeOnly", policy =>
                {
                    policy.RequireRole("Employee");
                });
            });

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowLocalhost4200", policy =>
            //    {
            //        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            //            .AllowAnyMethod()
            //              .AllowAnyHeader()
            //              .AllowCredentials();

            //    });
            //});


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Idenity Learing API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
            });



        }
    }
}
