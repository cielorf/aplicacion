using Backend.Api;
using Backend.Data.Models;
using Backend.Data.Models.MSSQL;
using Backend.Dependencies;
using Backend.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aplicacion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            builder.Services.AddDbContext<BdenviusaContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            });

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            builder.Services.AddScoped<MongoDbContext>(opt =>
            {
                var settings = opt.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoDbContext(settings.ConnectionString, settings.DatabaseName);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddScoped<IPeliculaDependencies, PeliculaDependencies>();
            builder.Services.AddScoped<PeliculaService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ReactPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}