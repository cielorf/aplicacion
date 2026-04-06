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
                // Esto permite que coexistan Backend.Data.Models.Serie y Backend.Data.Models.MSSQL.Serie
                options.CustomSchemaIds(type => type.FullName);
            });

            // Configuración de MongoDB
            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            builder.Services.AddScoped<MongoDbContext>(opt =>
            {
                var settings = opt.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoDbContext(settings.ConnectionString, settings.DatabaseName);
            });

            // Inyección de Dependencias y Servicios de Series
            //builder.Services.AddScoped<ISeriesDependenciesMongo, SeriesDependenciesMongo>();
            //builder.Services.AddScoped<SeriesService>();

            // Inyección de Dependencias y Servicios de Películas
            builder.Services.AddScoped<IPeliculaDependencies, PeliculaDependencies>();
            builder.Services.AddScoped<PeliculaService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}