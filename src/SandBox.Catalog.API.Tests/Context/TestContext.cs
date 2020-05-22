using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using WireMock.Server;
using WireMock.Settings;
using Xunit;

namespace SandBox.Catalog.API.Tests.Context
{
    public class TestContext : IDisposable, IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient HttpClient;
        public FluentMockServer MockServer { get; private set; }

        public TestContext(WebApplicationFactory<Startup> factory, int portNumber, bool useHttps)
        {
            MockServer = SetupMockedServer();

            Dictionary<string, string> extraConfiguration = GetConfiguration();
            string afterHttp = useHttps ? "s" : "";
            HttpClient = factory.WithWebHostBuilder(whb =>
            {
                whb.ConfigureAppConfiguration((context, configbuilder) =>
                {
                    configbuilder.AddInMemoryCollection(extraConfiguration);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri($"http{afterHttp}://localhost:{portNumber}")
            });
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
