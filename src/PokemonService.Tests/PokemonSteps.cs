using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
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

        [Given(@"a (.*) pokemon name")]
        public void GivenAValidPokemonName(string valid)
        {
            if (valid == "valid")
            {
                this.scenario.PokemonName = "pikachu";
            }
        }

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

        [Then(@"response body contains (.*)")]
        public void ThenResponseBodyContainsName(string filed)
        {
            // Assert.AreEqual(this.scenario.Response.Body, filed);
        }
    }
}