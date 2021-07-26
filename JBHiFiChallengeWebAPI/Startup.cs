using JBHiFiChallengeWebAPI.Helpers;
using JBHiFiChallengeWebAPI.ServiceContracts;
using JBHiFiChallengeWebAPI.ServiceImplementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JBHiFiChallengeWebAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var configPath = $"Config/";
            string envName = env.EnvironmentName;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"{configPath}appsettings.{envName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            AppDefs.AppSettings.OpenWeatherMapApiKey = appSettingsSection.GetValue<string>("OpenWeatherMapApiKey");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JBHiFiChallengeWebAPI", Version = "v1" });
                c.OperationFilter<Swagger.AddRequiredHeaderParameter>();
            });

            services.AddScoped<IRateLimitService, RateLimitService>();
            services.AddScoped<IRateLimitCheckService, RateLimitCheckServiceInMemory>();
            services.AddScoped<IWeatherMapService, WeatherMapService>();
            services.AddScoped<IWebCallService, WebCallService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JBHiFiChallengeWebAPI v1"));
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
