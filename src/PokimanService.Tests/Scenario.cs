using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PokimanService.Tests
{
    public class Scenario
    {

        private readonly WebApplicationFactory<PokimanService.Startup> factory;

        public Scenario(WebApplicationFactory<PokimanService.Startup> factory)
        {
            this.factory = factory;
        }

        public string PokimanName { get; set; }
        public HttpResponseMessage Response { get; set; }

        public async Task sendRequestAsync()
        {
            var client = this.factory.CreateClient();
            this.Response = await client.GetAsync($"/pokeman/{this.PokimanName}");
        }
    }
}