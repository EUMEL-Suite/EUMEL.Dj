using System;
using System.Text.Json.Serialization;
using Eumel.Dj.WebServer.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Eumel.Dj.WebServer
{
    public class Startup
    {
        private static string GetMajorMinorVersion()
        {
            var ver = typeof(Startup).Assembly.GetName().Version ?? new Version(0, 0);
            return $"{ver.Major}.{ver.Minor}";
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                // this will do the trick to make the PlayerControl enum available as string in swagger json.
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSignalR();
            services.AddSingleton<PlaylistAdapterService>();
            services.AddSingleton<ChatAdapterService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Constants.ApplicationName, Version = $"v{GetMajorMinorVersion()}" });
                c.UseAllOfToExtendReferenceSchemas();
                c.OperationFilter<SwaggerUserTokenHeader>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"{Constants.Swagger.JsonEndpoint}", $"{Constants.ApplicationName} v{GetMajorMinorVersion()}"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PlaylistHub>($"/{Constants.PlaylistHub.Route}").WithMetadata();
                endpoints.MapHub<ChatHub>($"/{Constants.ChatHub.Route}").WithMetadata();
            });

            // Start the adapter services so they link both hubs
            _ = app.ApplicationServices.GetService<PlaylistAdapterService>();
            _ = app.ApplicationServices.GetService<ChatAdapterService>();
        }
    }
}