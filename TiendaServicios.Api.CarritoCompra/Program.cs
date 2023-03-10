using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Application;
using TiendaServicios.Api.CarritoCompra.Repository;
using TiendaServicios.Api.CarritoCompra.ResourceInterfaces;
using TiendaServicios.Api.CarritoCompra.ResourceServices;

namespace TiendaServicios.Api.CarritoCompra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ContextoCarrito>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDB"));
            });

            builder.Services.AddMediatR(typeof(Nuevo.Handler).Assembly);

            builder.Services.AddHttpClient("Libros", config =>
            {
                config.BaseAddress = new Uri(builder.Configuration["Services:Libros"]);
            });

            builder.Services.AddScoped<ILibrosService, LibrosService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}