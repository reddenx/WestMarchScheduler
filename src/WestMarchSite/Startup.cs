using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WestMarchSite
{
    public class Startup
    {
        private IConfiguration Config { get; }

        public Startup(IConfiguration config)
        {
            Config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //base setup
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddMvc()
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "West Marches API", Version = "v1" });
            });

            //dependencies
            services.SetupDiContainer(Config);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(p => p
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("*")
                    .AllowCredentials());
            }

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "West Marches API");
            });

            app.UseAuthentication();

            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
