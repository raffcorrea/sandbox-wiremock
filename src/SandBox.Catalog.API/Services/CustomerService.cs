using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICepService _cepService;

        public CustomerService(ICepService cepService)
        {
            _cepService = cepService;
        }

        public async Task<CustomerModel> GetCustomerByCode(int code)
        {
            CustomerModel customerResponse = CustomerFakeRepository(c => c.Code == code)
                                                .FirstOrDefault();

            if(customerResponse != null && !string.IsNullOrEmpty(customerResponse.Cep))
                customerResponse.CepDetails = await _cepService.GetCepDetails(customerResponse.Cep);

            return customerResponse;
        }

        #region Private Method
        private IEnumerable<CustomerModel> CustomerFakeRepository(Func<CustomerModel, bool> predicate)
        {
            IList<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel { Code = 1, Name = "Customer 01", Cep = "95600106" },
                new CustomerModel { Code = 2, Name = "Customer 02", Cep = "95600104" }
            };

            return customers.Where(predicate);
        }
        #endregion
    }
}
