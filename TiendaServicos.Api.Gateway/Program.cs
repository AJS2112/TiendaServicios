using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TiendaServicos.Api.Gateway.MessageHandler;
using TiendaServicos.Api.Gateway.ResourceImplementations;
using TiendaServicos.Api.Gateway.ResourceInterfaces;

namespace TiendaServicos.Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("ocelot.json");
            // Add services to the container.

            //builder.Services.AddControllers();
            builder.Services.AddOcelot()
                .AddDelegatingHandler<LibroHandler>();

            builder.Services.AddHttpClient("AutorService", config =>
            {
                config.BaseAddress = new Uri(builder.Configuration["Services:Autor"]);
            });

            builder.Services.AddSingleton<IAutorResource, AutorResource>();    

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();


            //app.MapControllers();

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}