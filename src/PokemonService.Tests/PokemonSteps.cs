using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using PokemonService.Models;
using TechTalk.SpecFlow;

namespace PokemonService.Tests
{

    [Binding]
    [Scope(Feature = "GetPokemon")]
    public class PokemonSteps
    {

        public Scenario scenario;

        public PokemonSteps(Scenario scenario)
        {
            this.scenario = scenario;
        }

        [Before]
        public void Setup()
        {
            if (Environment.GetEnvironmentVariable("POKEMON_API_HOST") == null)
            {
                Environment.SetEnvironmentVariable("POKEMON_API_HOST", "http://localhost:6081");
            }
            if (Environment.GetEnvironmentVariable("TRANSLATE_API_HOST") == null)
            {
                Environment.SetEnvironmentVariable("TRANSLATE_API_HOST", "http://localhost:6082");
            }
            if (Environment.GetEnvironmentVariable("REDIS_HOST") == null)
            {
                Environment.SetEnvironmentVariable("REDIS_HOST", "http://localhost:6379");
            }
            if (Environment.GetEnvironmentVariable("MOUNTEBANK_HOST") == null)
            {
                Environment.SetEnvironmentVariable("MOUNTEBANK_HOST", "http://localhost:2525");
            }
        }

        [After]
        public void TearDown()
        {
            this.scenario.TearDownCache();
        }

        [Given(@"pokemon has (.*) details")]
        public void GivenPokemonApiHasDetails(string name)
        {
            this.scenario.MakePokemonApi(name);
        }

        [Given(@"translate api is up")]
        public void GivenAPIsAreUp()
        {
            this.scenario.MakeTranslateApi();
        }

        [Given(@"a (.*) pokemon name as (.*)")]
        public void GivenAValidPokemonName(string valid, string name)
        {
            if (valid == "valid")
            {
                this.scenario.PokemonName = name;
            }
            if (valid == "invalid")
            {
                this.scenario.PokemonName = Guid.NewGuid().ToString();
            }
        }

        [Given(@"get on pokemon end point is already called")]
        [When(@"get on pokemon end point is called")]
        public async Task WhenGetOnPokemonEndpointIsCalledAsync()
        {
            await this.scenario.sendRequestAsync();
        }

        [Then(@"(.*) status is return")]
        public void ThenStatusIsReturn(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    Assert.AreEqual(HttpStatusCode.OK, this.scenario.Response.StatusCode);
                    break;
                case 404:
                    Assert.AreEqual(HttpStatusCode.NotFound, this.scenario.Response.StatusCode);
                    break;
                case 500:
                    Assert.AreEqual(HttpStatusCode.ServiceUnavailable, this.scenario.Response.StatusCode);
                    break;
            }
        }

        [Then(@"response body contains pokimon model")]
        public async Task ThenResponseBodyContainsField()
        {
            string jsonString = await this.scenario.Response.Content.ReadAsStringAsync();
            Pokemon pokemon = JsonConvert.DeserializeObject<Pokemon>(jsonString);
            Assert.IsNotNull(pokemon);
            Assert.AreEqual(pokemon.Name, this.scenario.PokemonName);
            Assert.IsNotEmpty(pokemon.Description);
        }

        [Then(@"response should come from (.*)")]
        public void ThenResponseISFrom(string source)
        {
            Assert.AreEqual(source, this.scenario.Response.Headers.GetValues("X-Source").FirstOrDefault());
        }
    }
}