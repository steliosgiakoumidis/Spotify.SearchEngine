using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Serilog;
using Spotify.SearchEngine.Api.Utilities;
using Spotify.SearchEngine.Application;
using Spotify.SearchEngine.Application.Handlers;
using Spotify.SearchEngine.Application.Interfaces;
using Spotify.SearchEngine.Infrastructure.Services;
using Spotify.SearchEngine.Infrastructure.Utilities;

namespace Spotify.SearchEngine.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Warning()
                        .WriteTo.Console()
                        .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient(Constants.ClientName, client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddTransientHttpErrorPolicy(p => p.RetryAsync(3));

            services.AddMemoryCache();
            services.AddSingleton<ApplicationSettings>();
            services.AddSingleton<HelperMethods>();
            services.AddSingleton<Transformer>();
            services.AddSingleton<Validator>();
            services.AddScoped<IArtistsHandler, ArtistsHandler>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISearchService, SearchService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Spotify Search Engine", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(endpoints =>
            {
                endpoints.SwaggerEndpoint("/swagger/v1/swagger.json", "Spotify Search EngineService");
            });

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
