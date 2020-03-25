using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonService.Models;
using PokemonService.Queries;

namespace PokemonService.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {

        private readonly ILogger<PokemonController> logger;
        private readonly IQuery query;

        public PokemonController(ILogger<PokemonController> logger, IQuery query)
        {
            this.logger = logger;
            this.query = query;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Pokemon>> GetPokemonAsync(string name)
        {
            return await query.ExecuteAsync(name, this.HttpContext);
        }
    }
}
