using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Tests.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SandBox.Catalog.API.Tests
{
    [Collection("TestSharedContextCollection")]
    public class CatalogClientTests : TestContext
    {
        public CatalogClientTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task Should_Get_A_Valid_List_Of_Catalogs_With_HTTP_Status_OK()
        {
            string actualResponseContent;
            HttpResponseMessage response;
            List<CatalogModel> catalogsResponse;

            string mockedServerURL = MockServer.Urls.Single();
            HttpClient.BaseAddress = new Uri(mockedServerURL);

            response = await HttpClient.GetAsync("/api/catalog");

            actualResponseContent = await response.Content.ReadAsStringAsync();
            catalogsResponse = JsonConvert.DeserializeObject<List<CatalogModel>>(actualResponseContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(catalogsResponse.All(c => c.Name.startsWith("Mocked")));
        }
    }
}
