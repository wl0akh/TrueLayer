using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonService.Exceptions;
using PokemonService.Models;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace PokemonService.Queries
{
    public class Query : IQuery
    {
        private readonly ILogger<Query> logger;
        private IRedisCacheClient cacheClient;

        public Query(ILogger<Query> logger, IRedisCacheClient cacheClient)
        {
            this.cacheClient = cacheClient;
            this.logger = logger;
        }

        public async Task<ActionResult<Pokemon>> ExecuteAsync(string pokemonName, HttpContext context)
        {
            Pokemon pokemon;
            try
            {
                var cachedPokemon = await this.cacheClient.Db0.GetAsync<Pokemon>(pokemonName);
                if (cachedPokemon != null)
                {
                    context.Response.Headers.Add("X-Source", "cache");
                    return cachedPokemon;
                }

                context.Response.Headers.Add("X-Source", "origin");
                pokemon = await QueryHelper.Process(pokemonName, this.logger);
                await this.cacheClient.Db0.AddAsync(pokemonName, pokemon, DateTimeOffset.Now.AddMinutes(10));
            }
            catch (RedisConnectionException e)
            {
                this.logger.LogWarning($"Error connection Redis {e}");
                pokemon = await QueryHelper.Process(pokemonName, this.logger);
            }
            catch (ResourceNotFoundException e)
            {
                this.logger.LogError($"Error ResourceNotFoundException {e}");
                return new NotFoundResult();
            }
            catch (ForbiddenException e)
            {
                this.logger.LogError($"Error ForbiddenException {e}");
                return new ForbidResult();
            }

            return pokemon;
        }
    }
}