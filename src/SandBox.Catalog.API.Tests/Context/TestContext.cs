using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using WireMock.Server;
using WireMock.Settings;

namespace SandBox.Catalog.API.Tests.Context
{
    public class TestContext : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _webFactory;

        public HttpClient HttpClient { get; private set; }

        public FluentMockServer MockServer { get; private set; }

        public TestContext(WebApplicationFactory<Startup> factory)
        {
            _webFactory = factory;

            MockServer = SetupMockedServer();

            HttpClient = SetupApiClient();
        }

        private FluentMockServer SetupMockedServer()
        {
            FluentMockServerSettings settings = new FluentMockServerSettings()
            {
                Urls = new[] { "http://localhost:63751" },
                ReadStaticMappings = true,
            };

            FluentMockServer mockServer = FluentMockServer.Start(settings);
            mockServer.ReadStaticMappings("Mappings/");

            return mockServer;
        }

        private HttpClient SetupApiClient()
        {
            Dictionary<string, string> extraConfiguration = GetConfiguration();

            HttpClient httpClient = _webFactory.WithWebHostBuilder(whb =>
            {
                whb.ConfigureAppConfiguration((context, configbuilder) =>
                {
                    configbuilder.AddInMemoryCollection(extraConfiguration);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:5348")
            });

            return httpClient;
        }

        protected virtual Dictionary<string, string> GetConfiguration()
        {
            return new Dictionary<string, string>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MockServer.Stop();
                MockServer.Dispose();
                HttpClient.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
