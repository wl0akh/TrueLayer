using System;
using Microsoft.AspNetCore.Http;

namespace PokimanService.Tests
{
    public class Scenario
    {
        public string PokimanName { get; set; }
        public HttpContext Context { get; }

        public void sendRequest()
        {
        }
    }
}