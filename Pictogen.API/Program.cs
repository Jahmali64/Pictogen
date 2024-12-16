using Pictogen.Application;
using Pictogen.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Pictogen.API;

public static class Program {
    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped(typeof(CancellationToken) , serviceProvider => {
            IHttpContextAccessor httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            return httpContext.HttpContext?.RequestAborted ?? CancellationToken.None;
        });

        //Add JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => { options.TokenValidationParameters = new TokenValidationParameters {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                       ValidAudience = builder.Configuration["Jwt:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                   };
               });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
                                           // Define the Security Definition
                                           options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                                                                                                                 Name = "Authorization",
                                                                                                                 Type = SecuritySchemeType.Http,
                                                                                                                 Scheme = "bearer",
                                                                                                                 BearerFormat = "JWT",
                                                                                                                 In = ParameterLocation.Header,
                                                                                                                 Description = "JWT Authorization header using the Bearer scheme."
                                                                                                             });
                                           // Apply the Security Requirement globally
                                           options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                                                                                                             {
                                                                                                                 new OpenApiSecurityScheme {
                                                                                                                                               Reference = new OpenApiReference {
                                                                                                                                                                                    Type = ReferenceType.SecurityScheme,
                                                                                                                                                                                    Id = "Bearer"
                                                                                                                                                                                },
                                                                                                                                               Scheme = "bearer",
                                                                                                                                               Name = "Bearer",
                                                                                                                                               In = ParameterLocation.Header
                                                                                                                                           },
                                                                                                                 new List<string>() // Scopes - leave empty for JWT
                                                                                                             }
                                                                                                         });
                                       });

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
