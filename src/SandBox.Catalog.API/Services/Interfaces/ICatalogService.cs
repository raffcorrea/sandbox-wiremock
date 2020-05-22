using SandBox.Catalog.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogs();
    }
}
