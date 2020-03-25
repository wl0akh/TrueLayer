using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonService.Exceptions;
using PokemonService.Models;

namespace PokemonService.Queries
{
    public class QueryHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string pokemonApiHost = Environment.GetEnvironmentVariable("POKEMON_API_HOST");
        private static readonly string translateApiHost = Environment.GetEnvironmentVariable("TRANSLATE_API_HOST");

        public static async Task<Pokemon> Process(string pokemonName, ILogger logger)
        {
            var pokemon = new Pokemon
            {
                Name = pokemonName
            };

            try
            {
                string toBeTranslated = await GetPokemonDescriptionAsync(pokemonName, logger);
                string translation = await GetTranslationAsync(logger, toBeTranslated);
                pokemon.Description = translation;
            }
            catch (JsonReaderException e)
            {
                logger.LogError(e, $"Error processing Query");
            }
            return pokemon;
        }

        private static async Task<string> GetTranslationAsync(ILogger logger, string toBeTranslated)
        {
            var formContent = new FormUrlEncodedContent(
                    new[]{
                        new KeyValuePair<string, string>("text", toBeTranslated)
                    }
                );

            logger.LogDebug($"Making Post translate call to {translateApiHost}");

            var translatedResponse = await client.PostAsync(
                new Uri($"{translateApiHost}/translate/shakespeare.json"),
                formContent
                );

            if (translatedResponse.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ForbiddenException("Translation can't be preformed");
            }
            string jsonTranslationString = await translatedResponse.Content.ReadAsStringAsync();
            JObject transleted = JObject.Parse(jsonTranslationString);

            logger.LogDebug($"Add translation to Pokemon Model");

            string translation = transleted["contents"]["translated"].ToString();
            return translation;
        }

        private static async Task<string> GetPokemonDescriptionAsync(string pokemonName, ILogger logger)
        {
            logger.LogDebug($"Making Get pokemon-species call to {pokemonApiHost}");

            var pokemonResponse = await client.GetAsync(new Uri($"{pokemonApiHost}/api/v2/pokemon-species/{pokemonName}"));

            if (pokemonResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ResourceNotFoundException($"Pokemon not found {pokemonName} not found");
            }
            logger.LogDebug($"Succeed Get pokemon-species call to {pokemonApiHost}");

            string jsonString = await pokemonResponse.Content.ReadAsStringAsync();

            JObject flavorTextEntries = JObject.Parse(jsonString);
            IList<JToken> englishText = flavorTextEntries["flavor_text_entries"].Children().Where(x => "en".Equals(x["language"]["name"].ToString())).ToList();
            string toBeTranslated = englishText[0]["flavor_text"].ToString();
            return toBeTranslated;
        }
    }
}