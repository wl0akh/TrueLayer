using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokimanService.Models;

namespace PokimanService.Controllers
{
    [ApiController]
    [Route("pokeman")]
    public class PokimanController : ControllerBase
    {

        private readonly ILogger<PokimanController> _logger;

        public PokimanController(ILogger<PokimanController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{name}")]
        public Pokiman GetPokiman(string name)
        {
            return new Pokiman
            {
                Name = name,
                Description = Guid.NewGuid().ToString()
            };
        }
    }
}
