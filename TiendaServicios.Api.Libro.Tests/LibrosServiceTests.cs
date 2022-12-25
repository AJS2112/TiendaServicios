using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Application;
using TiendaServicios.Api.Libro.Dtos;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Repository;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTests
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>()
                .Setup(x => x.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriasMateriales).Returns(dbSet.Object);

            return contexto;
        }

        [Fact]
        public async void GetLibros()
        {
            var mockContext = CrearContexto();
            var mapConfig = new MapperConfiguration(c =>
            {
                c.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            var handler = new Consulta.Handler(mockContext.Object, mapper);

            var request = new Consulta.Query();

            var lista = await handler.Handle(request, new CancellationToken());

            Assert.True(lista.Any());
        }
    }
}
