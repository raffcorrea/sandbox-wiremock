using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Services
{
    public class CatalogService : ICatalogService
    {
        public Task<IEnumerable<CatalogModel>> GetCatalogs()
        {
            IEnumerable<CatalogModel> catalogs = new List<CatalogModel> { 
                new CatalogModel{ Code = 1, Name = "Notebooks", NumberOfProducts = 20 },
                new CatalogModel{ Code = 2, Name = "Keyboards", NumberOfProducts = 5 },
                new CatalogModel{ Code = 3, Name = "Monitors", NumberOfProducts = 8654 }
            };

            return Task.FromResult(catalogs);
        }
    }
}
