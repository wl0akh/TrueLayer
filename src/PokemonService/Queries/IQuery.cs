using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonService.Models;

namespace PokemonService.Queries
{
    public interface IQuery
    {
        Task<ActionResult<Pokemon>> ExecuteAsync(string pokemonName, HttpContext context);
    }
}