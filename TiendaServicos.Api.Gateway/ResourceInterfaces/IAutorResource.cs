using TiendaServicos.Api.Gateway.ResourceModels;

namespace TiendaServicos.Api.Gateway.ResourceInterfaces
{
    public interface IAutorResource
    {
        Task<(bool resultado, AutorResourceModel autor, string errorMessage)> GetAutor(Guid autorId);
    }
}
