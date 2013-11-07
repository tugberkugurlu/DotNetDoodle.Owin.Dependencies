using DotNetDoodle.Owin.Dependencies.Sample;
using DotNetDoodle.Owin.Dependencies.Sample.Repositories;
using Microsoft.Owin.Testing;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DotNetDoodle.Owin.Dependencies.Integration.Tests
{
    public class FullPipelineTests
    {
        [Fact]
        public async Task ShouldPassTheDependenciesCorrectlyForOwinMiddlewareEndpoint()
        {
            using (TestServer server = TestServer.Create<Startup>())
            {
                HttpResponseMessage response = await server.CreateRequest("/random").GetAsync();

                ConcurrentBag<string> bag;
                Startup.TypeOperations.TryGetValue(typeof(Repository), out bag);

                Assert.Equal(3, bag.Count);
            }
        }

        [Fact]
        public async Task ShouldPassTheDependenciesCorrectlyForOwinMiddlewareEndpointInParallel()
        {
            using (TestServer server = TestServer.Create<Startup>())
            {
                int concurrencyRate = 4;
                await Task.WhenAll(Enumerable.Range(0, concurrencyRate).Select(i => server.CreateRequest("/random").GetAsync()));

                ConcurrentBag<string> bag;
                Startup.TypeOperations.TryGetValue(typeof(Repository), out bag);

                Assert.Equal(3 * concurrencyRate, bag.Count);
            }
        }

        [Fact]
        public async Task ShouldPassTheDependenciesCorrectlyWebApiEndpoint()
        {
            using (TestServer server = TestServer.Create<Startup>())
            {
                HttpResponseMessage response = await server.CreateRequest("/api/texts").GetAsync();

                ConcurrentBag<string> bag;
                Startup.TypeOperations.TryGetValue(typeof(Repository), out bag);

                Assert.Equal(4, bag.Count);
            }
        }

        [Fact]
        public async Task ShouldPassTheDependenciesCorrectlyWebApiEndpointInParallel()
        {
            using (TestServer server = TestServer.Create<Startup>())
            {
                int concurrencyRate = 4;
                await Task.WhenAll(Enumerable.Range(0, concurrencyRate).Select(i => server.CreateRequest("/api/texts").GetAsync()));

                ConcurrentBag<string> bag;
                Startup.TypeOperations.TryGetValue(typeof(Repository), out bag);

                Assert.Equal(4 * concurrencyRate, bag.Count);
            }
        }
    }
}