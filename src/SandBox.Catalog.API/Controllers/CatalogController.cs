using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SandBox.Catalog.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            IEnumerable<CatalogModel> catalogs = await _catalogService.GetCatalogs();

            return Ok(catalogs);
        }
    }
}
