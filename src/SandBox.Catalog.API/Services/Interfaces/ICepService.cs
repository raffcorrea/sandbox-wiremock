using SandBox.Catalog.API.Models;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Services.Interfaces
{
    public interface ICepService
    {
        Task<CepModel> GetCepDetails(string cep);
    }
}
