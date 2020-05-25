using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using SandBox.Catalog.API.Tests.TestSupport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using WireMock.Server;
using WireMock.Settings;

namespace SandBox.Catalog.API.Tests.Context
{
    public class TestContext : IDisposable
    {
        #region Private Members
        private readonly WebApplicationFactory<Startup> _webFactory;
        private readonly TestSupportOptions _testSupportConfigurationOptions;
        #endregion

        public HttpClient HttpClient { get; private set; }

        public FluentMockServer MockServer { get; private set; }

        public TestContext(WebApplicationFactory<Startup> factory)
        {
            _webFactory = factory;

            _testSupportConfigurationOptions = GetTestSupportConfigurationOptions();

            MockServer = SetupMockedServer();

            HttpClient = SetupApiClient();
        }
        protected virtual Dictionary<string, string> GetApiClientExtraConfiguration()
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

        #region Private Methods
        private FluentMockServer SetupMockedServer()
        {
            FluentMockServerSettings settings = new FluentMockServerSettings()
            {
                Urls = new[] { _testSupportConfigurationOptions.ServerMockUrl },
                ReadStaticMappings = true,
            };

            FluentMockServer mockServer = FluentMockServer.Start(settings);
            mockServer.ReadStaticMappings("Mappings/");

            return mockServer;
        }

        private HttpClient SetupApiClient()
        {
            Dictionary<string, string> extraConfiguration = GetApiClientExtraConfiguration();

            HttpClient httpClient = _webFactory.WithWebHostBuilder(whb =>
            {
                whb.ConfigureAppConfiguration((context, configbuilder) =>
                {
                    configbuilder.AddInMemoryCollection(extraConfiguration);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(_testSupportConfigurationOptions.ApiClientUrl)
            });

            return httpClient;
        }

        private TestSupportOptions GetTestSupportConfigurationOptions()
        {
            TestSupportOptions options = new TestSupportOptions();

            IConfigurationRoot config = new ConfigurationBuilder()
                                        .SetBasePath(AppContext.BaseDirectory)
                                        .AddJsonFile("appsettings.json", false, true)
                                        .Build();

            config.GetSection(nameof(TestSupportOptions)).Bind(options);

            if (string.IsNullOrEmpty(options.ApiClientUrl) || string.IsNullOrEmpty(options.ServerMockUrl))
                throw new ConfigurationErrorsException($"Invalid configuration in appsettings json file");

            return options;
        }
        #endregion
    }
}
