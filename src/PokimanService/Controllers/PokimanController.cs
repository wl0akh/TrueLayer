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
    [Route("[controller]")]
    public class PokimanController : ControllerBase
    {

        private readonly ILogger<PokimanController> _logger;

        public PokimanController(ILogger<PokimanController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{Id}")]
        public async Task<Pokiman> GetPokimanAsync(string Id)
        {
            return new Pokiman
            {
                Name = Id,
                Description = Guid.NewGuid().ToString()
            };
        }
    }
}
