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
        private PlaylistAdapterService _pla;
        private object _ca;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EUMEL Dj", Version = "v1" });
                c.UseAllOfToExtendReferenceSchemas();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EUMEL Dj v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PlaylistHub>($"/{PlaylistHub.Route}").WithMetadata();
                endpoints.MapHub<ChatHub>($"/{ChatHub.Route}").WithMetadata();
            });

            _pla = app.ApplicationServices.GetService<PlaylistAdapterService>();
            _ca = app.ApplicationServices.GetService<ChatAdapterService>();
        }
    }
}