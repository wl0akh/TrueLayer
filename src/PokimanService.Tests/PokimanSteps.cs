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
        public void WhenGetOnPokimanEndpointIsCalled()
        {
            this.scenario.sendRequest();
        }

        [Then(@"(.*) status is return")]
        public void ThenStatusIsReturn(int statusCode)
        {
            // Assert.Equals(this.scenario.Context.Response.StatusCode, statusCode);
        }

        [Then(@"response body contains (.*)")]
        public void ThenResponseBodyContainsName(string filed)
        {
            // Assert.Equals(this.scenario.Context.Response.Body, filed);
        }
    }
}