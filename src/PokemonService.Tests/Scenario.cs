using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MbDotNet;
using MbDotNet.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace PokemonService.Tests
{
    public class Scenario
    {
        public IClient mounteBankClient;
        private readonly WebApplicationFactory<PokemonService.Startup> factory;

        public Scenario(WebApplicationFactory<PokemonService.Startup> factory)
        {
            this.factory = factory;
        }

        public string PokemonName { get; set; }
        public HttpResponseMessage Response { get; set; }


        public void TearDownCache()
        {
            var uri = new Uri(Environment.GetEnvironmentVariable("REDIS_HOST"));
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"{uri.Host}:{uri.Port},allowAdmin=true");
            IServer server = redis.GetServer(uri.Host, uri.Port);
            server.FlushDatabase();
            var mounteBankClient = new MountebankClient(Environment.GetEnvironmentVariable("MOUNTEBANK_HOST"));
            mounteBankClient.DeleteAllImposters();
        }

        public void MakePokemonApi(string pokemonName)
        {
            var mounteBankClient = new MountebankClient(Environment.GetEnvironmentVariable("MOUNTEBANK_HOST"));
            var uri = new Uri(Environment.GetEnvironmentVariable("POKEMON_API_HOST"));
            var pokemonApiPort = uri.Port;
            var imposter = mounteBankClient.CreateHttpImposter(pokemonApiPort);
            var pokemonDetailResponse = GeneratePokemonSpeciesDetailsResponseBody();
            imposter.AddStub().ReturnsJson(HttpStatusCode.OK, pokemonDetailResponse).OnPathEquals($"/api/v2/pokemon-species/{pokemonName}");

            mounteBankClient.Submit(imposter);
        }

        public async Task sendRequestAsync()
        {
            var client = this.factory.CreateClient();
            this.Response = await client.GetAsync($"/pokemon/{this.PokemonName}");
        }

        public void MakeTranslateApi()
        {
            var mounteBankClient = new MountebankClient(Environment.GetEnvironmentVariable("MOUNTEBANK_HOST"));
            var uri = new Uri(Environment.GetEnvironmentVariable("TRANSLATE_API_HOST"));
            var translateApiPort = uri.Port;
            var imposter = mounteBankClient.CreateHttpImposter(translateApiPort);

            var transelationResponse = new
            {
                success = new
                {
                    total = 1
                },
                contents = new
                {
                    translated = "Its nature is to store up electricity. Forests\\nwhere aeries of pikachu liveth art dangerous, \\nsince the trees art so oft did strike by lightning.",
                    text = "Its nature is to store up electricity. Forests\\nwhere nests of Pikachu live are dangerous,\\nsince the trees are so often struck by lightning.",
                    translation = "shakespeare"
                }
            };

            imposter.AddStub().ReturnsJson(HttpStatusCode.OK, transelationResponse)
                .OnPathAndMethodEqual("/translate/shakespeare.json", Method.Post);

            mounteBankClient.Submit(imposter);
        }

        private object GeneratePokemonSpeciesDetailsResponseBody()
        {
            return new
            {
                flavor_text_entries = new[]{
                    new{
                        flavor_text= "Its nature is to store up electricity. Forests\nwhere nests of Pikachu live are dangerous,\nsince the trees are so often struck by lightning.",
                        language = new
                        {
                            name = "en",
                            url = "https://pokeapi.co/api/v2/language/9/"
                        }
                    }
                }

            };
        }
    }
}