using Microsoft.EntityFrameworkCore;
using MikroCopUsers.Data;
using MikroCopUsers.Services;

namespace MikroCopUsers;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        

        // Add services to the container.
        // Add services to the container.
        builder.Services.AddControllers(); // Required for attribute-based controllers
        builder.Services.AddAuthorization();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Enter the API key here:",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "X-API-KEY",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });


        builder.Services.AddScoped<PasswordHasher>();
        var app = builder.Build();
        // Enable Swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI(); // Will serve Swagger UI at /swagger

        app.UseHttpsRedirection();
        app.UseMiddleware<ApiKeyMiddleware>();

        app.UseAuthorization();

        app.MapControllers(); // Important to route controller endpoints

        app.Run();
    }
}