using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace PokimanService.Tests
{

    [Binding]
    [Scope(Feature = "GetPokiman")]
    public class PokimanSteps
    {

        public Scenario scenario;

        public PokimanSteps(Scenario scenario)
        {
            this.scenario = scenario;
        }

        [Given(@"a (.*) pokiman name")]
        public void GivenAValidPokimanName(string valid)
        {
            if (valid == "valid")
            {
                this.scenario.PokimanName = "pikachu";
            }
        }

        [When(@"get on pokiman end point is called")]
        public async Task WhenGetOnPokimanEndpointIsCalledAsync()
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