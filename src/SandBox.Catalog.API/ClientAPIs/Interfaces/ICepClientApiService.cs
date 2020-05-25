using Refit;
using SandBox.Catalog.API.Models;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.ClientAPIs.Interfaces
{
    public interface ICepClientApiService
    {
        [Get("/ws/{cep}/json")]
        Task<CepModel> GetAddressAsync(string cep);
    }
}
