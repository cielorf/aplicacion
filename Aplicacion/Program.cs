using Backend.Api;
using Backend.Dependencies;
using Backend.Service;
using Backend.Data.Models;
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            builder.Services.AddScoped<MongoDbContext>(opt =>
            {
                var settings = opt.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoDbContext(settings.ConnectionString, settings.DatabaseName);
            });

            builder.Services.AddScoped<ISeriesDependencies, SeriesDependencies>();
            builder.Services.AddScoped<SeriesService>();

            builder.Services.AddScoped<IPeliculasDependencies, PeliculasDependencies>();
            builder.Services.AddScoped<PeliculasService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}