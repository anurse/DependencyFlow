using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Octokit;

namespace DependencyFlow
{
    public class Startup
    {
        private const string UserAgentValue = "DependencyFlow";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<swaggerClient>((services, client) =>
            {
                var authToken = Configuration["Maestro:Token"];
                if(string.IsNullOrEmpty(authToken))
                {
                    throw new InvalidOperationException("Missing required configuration option 'Maestro:Token'");
                }
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, UserAgentValue);
                client.DefaultRequestHeaders.Add(
                    HeaderNames.Authorization, 
                    new AuthenticationHeaderValue("Bearer", authToken).ToString());
            });

            services.AddOctokit(Configuration.GetSection("GitHub"));
            services.AddRazorPages();

            // Load site options
            services.Configure<DependencyFlowOptions>(Configuration.GetSection("DependencyFlow"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
