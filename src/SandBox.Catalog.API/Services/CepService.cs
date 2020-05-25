using Microsoft.Extensions.Options;
using Refit;
using SandBox.Catalog.API.ClientAPIs.Interfaces;
using SandBox.Catalog.API.ConfigurationOptions;
using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Services.Interfaces;
using System.Configuration;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Services
{
    public class CepService : ICepService
    {
        private readonly CepApiOptions _cepApiOptions;

        public CepService(IOptions<CepApiOptions> cepApiOptions)
        {
            _cepApiOptions = cepApiOptions.Value;
        }

        public async Task<CepModel> GetCepDetails(string cep)
        {
            if (_cepApiOptions == null)
                throw new ConfigurationErrorsException("Cep API Url setting must be configured!");

            ICepClientApiService client = RestService.For<ICepClientApiService>(_cepApiOptions.Url);

            CepModel cepDetails = await client.GetAddressAsync(cep);

            return cepDetails;
        }
    }
}
