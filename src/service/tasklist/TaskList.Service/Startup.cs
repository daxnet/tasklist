using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using TaskList.Common;
using TaskList.CrudProviders.MongoDB;

namespace TaskList.Service
{
    public class Startup
    {
        private readonly ILogger logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "TaskList Service",
                    Version = "v1",
                    Description = "The RESTful API for TaskList."
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xml = Path.Combine(basePath, "TaskList.Service.xml");
                if (File.Exists(xml))
                {
                    options.IncludeXmlComments(xml);
                }
            });

            RegisterApplicationServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskList Service");
            });

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMvc();
        }

        private void RegisterApplicationServices(IServiceCollection services)
        {
            var mongoServerHost = Configuration["mongo:server:host"];
            if (string.IsNullOrEmpty(mongoServerHost))
            {
                mongoServerHost = "localhost";
            }

            if (!int.TryParse(Configuration["mongo:server:port"], out var mongoServerPort))
            {
                mongoServerPort = 27017;
            }

            var mongoDatabase = Configuration["mongo:server:database"];
            if (string.IsNullOrEmpty(mongoDatabase))
            {
                mongoDatabase = "tasklist";
            }

            services.AddTransient<ICrudProvider>(serviceProvider => new MongoCrudProvider(mongoDatabase, mongoServerHost, mongoServerPort));
        }
    }
}
