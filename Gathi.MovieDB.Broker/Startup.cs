using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gathi.MovieDB.Broker.Config;
using Gathi.MovieDB.Broker.ServiceClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gathi.MovieDB.Broker
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create a named same origin policy that can be anotated @ Controller
            services.AddCors(o => o.AddPolicy("GathiPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Get the config associated with Artists controller and inject into the our DI Container
            services.Configure<MoviesControllerConfig>(Configuration.GetSection("MoviesControllerConfig"));

            // Create named client for our specs
            services.AddHttpClient("MovieDBClient", client =>
            {
                int timeout = int.Parse(Configuration["ClientTimeout"]);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "MovieDB-Client");

            });

            // Inject the client for service request
            services.AddScoped<IMovieDBClient, MovieDBClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
