using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PokemonService.Tests
{
    public class Scenario
    {

        private readonly WebApplicationFactory<PokemonService.Startup> factory;

        public Scenario(WebApplicationFactory<PokemonService.Startup> factory)
        {
            this.factory = factory;
        }

        public string PokemonName { get; set; }
        public HttpResponseMessage Response { get; set; }

        public async Task sendRequestAsync()
        {
            var client = this.factory.CreateClient();
            this.Response = await client.GetAsync($"/pokemon/{this.PokemonName}");
        }
    }
}