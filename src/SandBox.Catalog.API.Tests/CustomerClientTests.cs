﻿using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SandBox.Catalog.API.Models;
using SandBox.Catalog.API.Tests.Context;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SandBox.Catalog.API.Tests
{
    [Collection("TestSharedContextCollection")]
    public class CustomerClientTests : TestContext
    {
        private const string CEP_API_URL_SECTION = "CepApiOptions:Url";

        public CustomerClientTests(WebApplicationFactory<Startup> factory) 
            : base(factory)
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
        /// This method creates configurations using mockserver url, that will be used by the API instead of configured in appsettings, this setup allows Wire Mock Server matches the http request mapped.
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, string> GetApiClientExtraConfiguration()
        {
            string requestUrl = MockServer.Urls.Single();
            Dictionary<string, string> configuration = base.GetApiClientExtraConfiguration();
            configuration.Add(CEP_API_URL_SECTION, requestUrl);
            return configuration;
        }
    }
}
