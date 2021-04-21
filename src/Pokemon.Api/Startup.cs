using Amazon.Extensions.NETCore.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonCore.Core;
using PokemonCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonService.Api
{
    public class Startup
    {
        public static IWebHostEnvironment Environment { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddDefaultAWSOptions(new AWSOptions());
            
            services.AddMemoryCache();
            services.AddSingleton<IHttpClient, HttpClient>();
            services.AddSingleton<IPokemonProvider, PokemonProvider>();
            services.AddSingleton<IQuery<PokemonResponse>, GetPokemonQuery>();
            services.AddSingleton<IQueryHandler<GetPokemonQuery, PokemonResponse>>();
        }
    }
}
