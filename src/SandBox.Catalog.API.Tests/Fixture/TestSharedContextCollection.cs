using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SandBox.Catalog.API.Tests.Fixture
{
    [CollectionDefinition("TestSharedContextCollection")]
    public class TestSharedContextCollection : ICollectionFixture<WebApplicationFactory<Startup>>
    {
    }
}
