using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Application;
using TiendaServicios.Api.Autor.RabbitMqHandler;
using TiendaServicios.Api.Autor.Repository;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventQueue;
using TiendaServicios.RabbitMQ.Bus.Implementations;

namespace TiendaServicios.Api.Autor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ContextoAutor>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase"),
                    options => options.SetPostgresVersion(new Version(9, 6)));
            });

            //MediatR
            builder.Services.AddMediatR(typeof(Nuevo.Handler).Assembly);

            //FluentValidation
            builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            //AutoMapper
            builder.Services.AddAutoMapper(typeof(Consulta.Handler));

            builder.Services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });

            builder.Services.AddTransient<EmailEventoHandler>();

            builder.Services.AddTransient<IEventoHandler<EmailEventoQueue>, EmailEventoHandler>();

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

            var eventBus = app.Services.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<EmailEventoQueue, EmailEventoHandler>();

            app.Run();
        }
    }
};