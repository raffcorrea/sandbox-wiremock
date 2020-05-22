using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Tests.Context;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SandBox.Catalog.API.Tests
{
    public class CustomerClientTests : TestContext
    {
        public CustomerClientTests(WebApplicationFactory<Startup> factory) 
            : base(factory, 5347, false)
        {
        }

        [Theory]
        [Trait("Category", "Unit")]
        [InlineData("1")]
        [InlineData("2")]
        public async Task Should_Get_A_Valid_Customer_With_CepDetails_For_Customer_Code(string customerCode)
        {
            string actualResponseContent;
            HttpResponseMessage response;
            CustomerModel customerResponse;

            response = await HttpClient.GetAsync($"/api/Customer/{customerCode}");
            response.EnsureSuccessStatusCode();

            actualResponseContent = await response.Content.ReadAsStringAsync();
            customerResponse = JsonConvert.DeserializeObject<CustomerModel>(actualResponseContent);

            Assert.Equal("Mocked", customerResponse.CepDetails.Localidade);
        }

        /// <summary>
        /// This method create a Url configuration using mockserver url to be used by the API, this allow WireMockServer match the http request mapped.
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, string> GetConfiguration()
        {
            string requestUrl = MockServer.Urls.Single();
            Dictionary<string, string> configuration = base.GetConfiguration();
            configuration.Add("CepApiOptions:Url", requestUrl);
            return configuration;
        }
    }
}
