using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PokemonService.Queries;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace PokemonService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureEnvVar();
            RedisConfiguration redisConfiguration = ConfigureRedis();
            services.AddSingleton(redisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<ISerializer, NewtonsoftSerializer>();
            services.AddControllers();
            services.AddSingleton<IQuery, Query>();
        }

        private void ConfigureEnvVar()
        {
            if (Environment.GetEnvironmentVariable("POKEMON_API_HOST") == null)
            {
                Environment.SetEnvironmentVariable("POKEMON_API_HOST", "https://pokeapi.co");
            }
            if (Environment.GetEnvironmentVariable("TRANSLATE_API_HOST") == null)
            {
                Environment.SetEnvironmentVariable("TRANSLATE_API_HOST", "https://api.funtranslations.com");
            }
            if (Environment.GetEnvironmentVariable("REDIS_HOST") == null)
            {
                Environment.SetEnvironmentVariable("REDIS_HOST", "http://localhost:6379");
            }
        }

        private static RedisConfiguration ConfigureRedis()
        {
            var uri = new Uri(Environment.GetEnvironmentVariable("REDIS_HOST"));
            var redisConfiguration = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                KeyPrefix = "true_layer",
                Hosts = new RedisHost[] { new RedisHost() { Host = uri.Host, Port = uri.Port } },
                AllowAdmin = true,
                ConnectTimeout = 3000,
                Database = 0
            };
            return redisConfiguration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
