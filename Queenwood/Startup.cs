using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using NetEscapades.AspNetCore.SecurityHeaders;
using Queenwood.Core.Client.Etsy;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.EmailService;
using Queenwood.Models.Config;

namespace Queenwood
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get config settings
            services.Configure<EmailConfig>(Configuration.GetSection("Emails"));
            services.Configure<EtsyConfig>(Configuration.GetSection("Etsy"));


            // Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });


            // Cache on server based on client cache rules
            services.AddResponseCaching();

            // Client side caching profiles, keep small because no way to invalidate
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        VaryByHeader = "Accept-Encoding, X-QD-SV",
                        Location = ResponseCacheLocation.Any,
                        Duration = 60
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            });


            //services.AddSession();

            // Add application services
            services.AddSingleton<ICacheService, CacheService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEtsyClient, EtsyClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add default security headers
            var policyCollection = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders();

            app.UseSecurityHeaders(policyCollection);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                // output caching in production
                app.UseResponseCaching();
            }

            // Gzip
            app.UseResponseCompression();

            // add cache control header to static resources
            app.UseStaticFiles(new StaticFileOptions()
            {

                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(365),
                        Public = true
                    };
                }
            });

            // Enable sessions
            //app.UseSession();            

            // don't pass routes for Attribute routing
            app.UseMvc();
        }
    }
}

