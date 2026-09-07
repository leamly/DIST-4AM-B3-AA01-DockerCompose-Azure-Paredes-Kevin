
using InventarioA.Api.Data;
using InventarioA.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace InventarioA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddHostedService<RabbitMQConsumer>();


            builder.Services.AddDbContext<InventarioDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("InventarioConnection")));


            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();

            // configuracion de swagger con autenticacion y JWT
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Ingrese el token JWT"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
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

            // activación de autenticación			
            app.UseAuthentication();


            app.MapControllers();

            app.Run();
        }
    }
}
