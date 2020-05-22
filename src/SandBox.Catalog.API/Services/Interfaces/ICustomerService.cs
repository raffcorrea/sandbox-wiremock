using SandBox.Catalog.API.Models;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerModel> GetCustomerByCode(int code);
    }
}
