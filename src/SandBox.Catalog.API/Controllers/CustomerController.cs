using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Services.Interfaces;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{code:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int code)
        {
            CustomerModel customerResponse = await _customerService.GetCustomerByCode(code);

            return Ok(customerResponse);
        }
    }
}
